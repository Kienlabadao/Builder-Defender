using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
    [SerializeField] private ProjectileSO projectileSO;
    [SerializeField] private float despawnTimerMax = 2f;

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float despawnTimer;
    private float baseDamage;

    private void Awake() {
        despawnTimer = despawnTimerMax;
    }

    private void Update() {
        HandleMovement();
        HandleDespawn();
    }

    private void HandleMovement() {
        Vector3 moveDir;

        if (targetEnemy != null) {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
        } else {
            moveDir = lastMoveDir;
        }

        lastMoveDir = moveDir;

        transform.position += projectileSO.projectileSpeed * Time.deltaTime * moveDir;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));
    }

    private void HandleDespawn() {
        despawnTimer -= Time.deltaTime;

        if (despawnTimer <= 0f) {
            Destroy(gameObject);
        }
    }

    private float CalculateDamage() {
        return baseDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision2D) {
        if (collision2D.gameObject.TryGetComponent(out Enemy enemy)) {
            enemy.Damage(CalculateDamage());

            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy enemy) {
        targetEnemy = enemy;
    }

    private void SetBaseDamage(float baseDamage) {
        this.baseDamage = baseDamage;
    }

    public static ArrowProjectile SpawnProjectile(ProjectileSO projectileSO, Vector3 position, Enemy targetEnemy, float baseDamage) {
        Transform projectileTransform = Instantiate(projectileSO.prefab, position, Quaternion.identity);
        ArrowProjectile arrowProjectile = projectileTransform.GetComponent<ArrowProjectile>();

        arrowProjectile.SetTarget(targetEnemy);
        arrowProjectile.SetBaseDamage(baseDamage);

        return arrowProjectile;
    }
}
