using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject {
    public string enemyName;
    public Enemy enemyPrefab;
    public float healthAmountMax = 100f;
    public float moveSpeed = 6f;
    public float damage = 10f;
    public float lookForTargetTimerMax = .2f;
    public int poolInitialSize = 50;
    public int poolMaxSize = 200;
}
