using UnityEngine;

public class ResourceBuilding : MonoBehaviour {
    [SerializeField] private BuildingTypeSO buildingTypeSO;

    public BuildingTypeSO GetBuildingTypeSO() {
        return buildingTypeSO;
    }
}
