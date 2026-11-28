using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    private class EnemyPoolData {
        public PrefabPool<Enemy> enemyPool;
        public GameObject poolParent;
    }

    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField] private EnemyListSO enemyListSO;
    private Dictionary<EnemySO, EnemyPoolData> enemyPoolDataDictionary;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one EnemySpawnManager instance!");
        }
        Instance = this;

        enemyPoolDataDictionary = new Dictionary<EnemySO, EnemyPoolData>();

        foreach (EnemySO enemySO in enemyListSO.enemySOArray) {
            GameObject poolParent = new GameObject($"{enemySO.name}Pool");
            poolParent.transform.SetParent(transform);

            PrefabPool<Enemy> enemyPool = new PrefabPool<Enemy>(
                enemySO.enemyPrefab,
                poolParent.transform,
                enemySO.poolInitialSize,
                enemySO.poolMaxSize
            );

            enemyPoolDataDictionary[enemySO] = new EnemyPoolData {
                enemyPool = enemyPool,
                poolParent = poolParent
            };
        }
    }

    public Enemy SpawnEnemy(EnemySO enemySO, Vector3 position) {
        EnemyPoolData enemyPoolData = enemyPoolDataDictionary[enemySO];
        PrefabPool<Enemy> enemyPool = enemyPoolData.enemyPool;

        Enemy enemy = enemyPool.Get();
        enemy.Initialize(position, () => {
            enemyPool.Release(enemy);
        });

        return enemy;
    }
}
