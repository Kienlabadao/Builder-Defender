using UnityEngine;

[RequireComponent(typeof(Building))]
public class DefenseBuilding : MonoBehaviour {
    [SerializeField] private Transform projectileSpawnPosition;

    private DefenseBuildingData defenseBuildingData;
    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float attackTimer;

    private void Awake() {
        defenseBuildingData = GetComponent<Building>().GetBuildingSO().defenseBuildingData;
    }

    private void Update() {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;

        if (lookForTargetTimer <= 0f) {
            lookForTargetTimer = defenseBuildingData.lookForTargetTimerMax;

            LookForTarget();
        }
    }

    private void LookForTarget() {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, defenseBuildingData.range);
        Enemy previousTargetEnemy = targetEnemy;

        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.gameObject.TryGetComponent(out Enemy enemy)) {
                if (targetEnemy == null) {
                    targetEnemy = enemy;
                } else if (Vector3.Distance(transform.position, enemy.transform.position) <
                    Vector3.Distance(transform.position, targetEnemy.transform.position)) {
                    targetEnemy = enemy;
                }
            }
        }

        // Reset timer everytime defense found a new target
        if (targetEnemy != null && previousTargetEnemy != targetEnemy) {
            attackTimer = defenseBuildingData.attackSpeed;
        }
    }

    private void HandleShooting() {
        if (targetEnemy != null) {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f) {
                attackTimer = defenseBuildingData.attackSpeed;

                ArrowProjectile.SpawnProjectile(
                    defenseBuildingData.projectileSO,
                    projectileSpawnPosition.position,
                    targetEnemy,
                    defenseBuildingData.baseDamage
                );
            }
        }
    }
}
