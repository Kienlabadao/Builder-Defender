using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(TargetingSystem))]
public class Enemy : MonoBehaviour, IHasHealth, IDamageable, ITargetable, IRequiresTargetSystem {
    private const float TARGET_MAX_RADIUS = 10f;

    [SerializeField] private EnemySO enemySO;

    private HealthSystem healthSystem;
    private TargetingSystem targetingSystem;
    private Rigidbody2D rigidbody2D;
    private Transform targetTransform;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        targetingSystem = GetComponent<TargetingSystem>();
    }

    private void Start() {
        healthSystem.OnDied += HealthSystem_OnDied;

        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
    }

    private void Update() {
        HandleTargeting();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void OnDestroy() {
        healthSystem.OnDied -= HealthSystem_OnDied;
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (collision2D.gameObject.TryGetComponent(out Building building)) {
            building.Damage(enemySO.damage);

            Destroy(gameObject);
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Destroy(gameObject);
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
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }

    public float GetHealthAmountMax() {
        return enemySO.healthAmountMax;
    }

    public void Damage(float damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    public Vector3 GetTargetPosition() {
        return transform.position;
    }

    public static Enemy SpawnEnemy(EnemySO enemySO, Vector3 position) {
        Transform enemyTransform = Instantiate(enemySO.prefab, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();

        return enemy;
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
}
