using UnityEngine;

[CreateAssetMenu(fileName = "SpawnDatabaseSO", menuName = "Scriptable Objects/SpawnDatabaseSO")]
public class SpawnDatabaseSO : ScriptableObject {
    public BuildingConstruction buildingConstructionPrefab;
    public Projectile projectilePrefab;
}
