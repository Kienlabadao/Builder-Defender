using UnityEngine;

[CreateAssetMenu(fileName = "BuildingTypeSO", menuName = "Scriptable Objects/BuildingTypeSO")]
public class BuildingTypeSO : ScriptableObject {
    public string buildingName;
    public Transform prefab;
}
