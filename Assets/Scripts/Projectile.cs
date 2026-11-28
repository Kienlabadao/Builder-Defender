using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float despawnTimerMax = 2f;

    private ProjectileSO projectileSO;
    private Enemy targetEnemy = null;
    private int targetSpawnID;
    private Vector3 lastMoveDir;
    private float despawnTimer;
    private float baseDamage;
    private bool isInitialized = false;
    private Action onDespawnAction;

    private void Awake() {
        despawnTimer = despawnTimerMax;
    }

    private void Update() {
        if (isInitialized) {
            HandleMovement();
            HandleDespawn();
        }
    }

    private void OnDisable() {
        onDespawnAction = null;
        isInitialized = false;
    }

    private void OnTriggerEnter2D(Collider2D collision2D) {
        if (collision2D.gameObject.TryGetComponent(out Enemy enemy)) {
            enemy.Damage(CalculateDamage());

            Despawn();
        }
    }

    private void HandleMovement() {
        Vector3 moveDir;

        if (targetEnemy != null &&
            targetEnemy.gameObject.activeSelf &&
            targetEnemy.GetSpawnID() == targetSpawnID
        ) {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
        } else {
            moveDir = lastMoveDir;
        }

        lastMoveDir = moveDir;

        transform.position += projectileSO.projectileSpeed * Time.deltaTime * moveDir;
        HandleRotation(moveDir);
    }

    private void HandleRotation(Vector3 moveDir) {
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));
    }

    private void InitializeRotation(Enemy targetEnemy) {
        HandleRotation((targetEnemy.transform.position - transform.position).normalized);
    }

    private void HandleDespawn() {
        despawnTimer -= Time.deltaTime;

        if (despawnTimer <= 0f) {
            Despawn();
        }
    }

    private float CalculateDamage() {
        return baseDamage;
    }

    private void Despawn() {
        Action action = onDespawnAction;

        onDespawnAction = null;
        isInitialized = false;

        action?.Invoke();
    }



    private void SetTarget(Enemy enemy) {
        targetEnemy = enemy;
        targetSpawnID = enemy.GetSpawnID();
    }

    private void SetBaseDamage(float baseDamage) {
        this.baseDamage = baseDamage;
    }

    public void Initialize(
        ProjectileSO projectileSO,
        Vector3 position,
        Enemy targetEnemy,
        float baseDamage,
        Action onDespawnAction
    ) {
        transform.position = position;
        this.projectileSO = projectileSO;
        this.onDespawnAction = onDespawnAction;
        despawnTimer = despawnTimerMax;

        SetTarget(targetEnemy);
        SetBaseDamage(baseDamage);
        InitializeRotation(targetEnemy);

        isInitialized = true;
    }
}
