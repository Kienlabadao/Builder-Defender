using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    private const float TARGET_MAX_RADIUS = 10f;

    [SerializeField] private EnemyTypeSO enemyTypeSO;

    private Rigidbody2D rigidbody2D;
    private Transform targetTransform;
    private float lookForTargetTimer;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();

        lookForTargetTimer = Random.Range(0f, enemyTypeSO.lookForTargetTimerMax);
    }

    private void Start() {
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
    }

    private void Update() {
        HandleTargeting();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (collision2D.gameObject.TryGetComponent(out Building building)) {
            building.Damage(enemyTypeSO.damage);

            Destroy(gameObject);
        }
    }

    private void HandleMovement() {
        if (targetTransform != null) {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            rigidbody2D.linearVelocity = enemyTypeSO.moveSpeed * moveDir;
        } else {
            rigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;

        if (lookForTargetTimer <= 0) {
            lookForTargetTimer = enemyTypeSO.lookForTargetTimerMax;

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

    public static Enemy SpawnEnemy(EnemyTypeSO enemyTypeSO, Vector3 position) {
        Transform enemyTransform = Instantiate(enemyTypeSO.prefab, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();

        return enemy;
    }
}
