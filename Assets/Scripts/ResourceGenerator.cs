using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
    private ResourceTypeSO resourceTypeSO;
    private float generateResouseTimer;
    private float generateResouseTimerMax;

    private void Awake() {
        ResourceBuilding resourceBuilding = GetComponent<ResourceBuilding>();
        BuildingTypeSO buildingTypeSO = resourceBuilding.GetBuildingTypeSO();
        resourceTypeSO = buildingTypeSO.resourceGeneratorData.resourceTypeSO;
        generateResouseTimerMax = buildingTypeSO.resourceGeneratorData.generateResouseTimerMax;
    }

    private void Update() {
        generateResouseTimer -= Time.deltaTime;

        if (generateResouseTimer <= 0) {
            generateResouseTimer = generateResouseTimerMax;

            ResourceManager.Instance.AddResource(resourceTypeSO, 1);
        }
    }
}
