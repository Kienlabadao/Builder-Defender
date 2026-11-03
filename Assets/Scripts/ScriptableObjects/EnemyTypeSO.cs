using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeSO", menuName = "Scriptable Objects/EnemyTypeSO")]
public class EnemyTypeSO : ScriptableObject {
    public string enemyName;
    public Transform prefab;
    public float healthAmountMax = 100f;
    public float moveSpeed = 6f;
    public float damage = 10f;
    public float lookForTargetTimerMax = .2f;
}
