using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthSystem))]
public class Enemy : MonoBehaviour, IHasHealth, IDamageable {
    private const float TARGET_MAX_RADIUS = 10f;

    [SerializeField] private EnemySO enemySO;

    private HealthSystem healthSystem;
    private Rigidbody2D rigidbody2D;
    private Transform targetTransform;
    private float lookForTargetTimer;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();

        lookForTargetTimer = Random.Range(0f, enemySO.lookForTargetTimerMax);
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
        lookForTargetTimer -= Time.deltaTime;

        if (lookForTargetTimer <= 0f) {
            lookForTargetTimer = enemySO.lookForTargetTimerMax;

            LookForTarget();
        }
    }

    private void LookForTarget() {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, TARGET_MAX_RADIUS);

        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.gameObject.TryGetComponent(out Building building)) {
                if (targetTransform == null) {
                    targetTransform = building.transform;
                } else if (Vector3.Distance(transform.position, building.transform.position) <
                    Vector3.Distance(transform.position, targetTransform.position)) {
                    targetTransform = building.transform;
                }
            }
        }

        if (targetTransform == null) {
            // No targets wuthin range
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }

    public float GetHealthAmountMax() {
        return enemySO.healthAmountMax;
    }

    public void Damage(float damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    public static Enemy SpawnEnemy(EnemySO enemySO, Vector3 position) {
        Transform enemyTransform = Instantiate(enemySO.prefab, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();

        return enemy;
    }
}
