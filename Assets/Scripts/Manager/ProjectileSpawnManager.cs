using UnityEngine;

public class ProjectileSpawnManager : MonoBehaviour {
    public static ProjectileSpawnManager Instance { get; private set; }

    private const int PROJECTILE_POOL_INITIAL_SIZE = 20;
    private const int PROJECTILE_POOL_MAX_SIZE = 100;

    private PrefabPool<Projectile> projectilePool;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one ProjectileSpawnManager instance!");
        }
        Instance = this;
    }

    private void Start() {
        projectilePool = new PrefabPool<Projectile>(
            SpawnDatabase.GetSpawnDatabaseSO().projectilePrefab,
            transform,
            PROJECTILE_POOL_INITIAL_SIZE,
            PROJECTILE_POOL_MAX_SIZE
        );
    }

    public Projectile SpawnProjectile(
        ProjectileSO projectileSO,
        Vector3 position,
        Enemy targetEnemy,
        float baseDamage
    ) {
        Projectile projectile = projectilePool.Get();
        projectile.Initialize(projectileSO, position, targetEnemy, baseDamage, () => {
            projectilePool.Release(projectile);
        });

        return projectile;
    }
}
