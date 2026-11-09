using UnityEngine;

[RequireComponent(typeof(Building))]
[RequireComponent(typeof(TargetingSystem))]
public class DefenseBuilding : MonoBehaviour, IRequiresTargetSystem {
    [SerializeField] private Transform projectileSpawnPosition;

    private TargetingSystem targetingSystem;
    private DefenseBuildingData defenseBuildingData;
    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float attackTimer;

    private void Awake() {
        targetingSystem = GetComponent<TargetingSystem>();
        defenseBuildingData = GetComponent<Building>().GetBuildingSO().defenseBuildingData;
    }

    private void Update() {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleTargeting() {
        Enemy previousTargetEnemy = targetEnemy;

        targetEnemy = targetingSystem.GetCurrentTarget() as Enemy;

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

    public Transform GetTargetSearchOrigin() {
        return transform;
    }

    public float GetMaxRange() {
        return defenseBuildingData.range;
    }

    public TargetType GetTargetType() {
        return TargetType.Enemy;
    }

    public float GetTargetScanInterval() {
        return defenseBuildingData.lookForTargetTimerMax;
    }
}
