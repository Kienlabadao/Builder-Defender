using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(TargetingSystem))]
public class Enemy : MonoBehaviour, IHasHealth, IDamageable, ITargetable, IRequiresTargetSystem {
    private static int nextSpawnId;

    private const float TARGET_MAX_RADIUS = 10f;

    public event EventHandler OnInitializeCompleted;
    public event EventHandler OnDamaged;

    [SerializeField] private EnemySO enemySO;

    private int spawnId;
    private HealthSystem healthSystem;
    private TargetingSystem targetingSystem;
    private Rigidbody2D rigidbody2D;
    private Transform targetTransform;
    private Action onDieAction;
    private bool isInitialized = false;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        targetingSystem = GetComponent<TargetingSystem>();

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void Start() {
        SetDefaultTarget();
    }

    private void Update() {
        if (isInitialized) {
            HandleTargeting();
        }
    }

    private void FixedUpdate() {
        if (isInitialized) {
            HandleMovement();
        }
    }

    private void OnDisable() {
        onDieAction = null;
        isInitialized = false;
    }

    private void OnDestroy() {
        healthSystem.OnDied -= HealthSystem_OnDied;
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (collision2D.gameObject.TryGetComponent(out Building building)) {
            building.Damage(enemySO.damage);

            Die();
        }
    }

    private void HealthSystem_OnDied(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);

        Die();
    }

    private void HandleMovement() {
        if (targetTransform != null) {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            rigidbody2D.linearVelocity = enemySO.moveSpeed * moveDir;
        } else {
            rigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    private void HandleTargeting() {
        if (targetingSystem.GetCurrentTarget() is Building building && building != null) {
            targetTransform = building.transform;
        } else {
            SetDefaultTarget();
        }
    }

    private void SetDefaultTarget() {
        Building hqBuilding = BuildingManager.Instance.GetHQBuilding();

        if (hqBuilding != null) {
            targetTransform = hqBuilding.transform;
        }
    }

    private void Die() {
        Action action = onDieAction;

        onDieAction = null;
        isInitialized = false;

        action?.Invoke();
    }

    public void Initialize(Vector3 position, Action onDieAction) {
        transform.position = position;
        this.onDieAction = onDieAction;
        spawnId = ++nextSpawnId;

        healthSystem.ResetHealthSystem();
        targetingSystem.ResetTargetingSystem();

        OnInitializeCompleted?.Invoke(this, EventArgs.Empty);
        isInitialized = true;
    }

    public float GetHealthAmountMax() {
        return enemySO.healthAmountMax;
    }

    public float GetHealthAmount() {
        return healthSystem.GetHealthAmount();
    }

    public float GetMissingHealthAmount() {
        return GetHealthAmountMax() - GetHealthAmount();
    }

    public bool IsFullHealth() {
        return healthSystem.IsFullHealth();
    }

    public void Damage(float damageAmount) {
        healthSystem.Damage(damageAmount);

        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);

        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetTargetPosition() {
        return transform.position;
    }

    public Transform GetTargetSearchOrigin() {
        return transform;
    }

    public float GetMaxRange() {
        return TARGET_MAX_RADIUS;
    }

    public TargetType GetTargetType() {
        return TargetType.Building;
    }

    public float GetTargetScanInterval() {
        return enemySO.lookForTargetTimerMax;
    }

    public int GetSpawnID() {
        return spawnId;
    }
}
