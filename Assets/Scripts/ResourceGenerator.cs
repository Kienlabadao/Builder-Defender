using UnityEngine;

[RequireComponent(typeof(ResourceBuilding))]
public class ResourceGenerator : MonoBehaviour {
    [SerializeField] private float maxTimerReduction = .5f;

    private ResourceGeneratorData resourceGeneratorData;
    private float generateResouseTimer;
    private float generateResouseTimerMax;


    private void Awake() {
        resourceGeneratorData = GetComponent<ResourceBuilding>().GetBuildingTypeSO().resourceGeneratorData;
    }

    private void Start() {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;

        int nearbyResourceNodeAmount = 0;
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out ResourceNode resourceNode) && resourceNode.GetResourceTypeSO() == resourceGeneratorData.resourceTypeSO) {
                nearbyResourceNodeAmount++;
            }
        }
        nearbyResourceNodeAmount = Mathf.Clamp(nearbyResourceNodeAmount, 0, maxResourceAmount);

        if (nearbyResourceNodeAmount == 0) {
            enabled = false;
        } else {
            // Calculate timer reduction percentage based on the resource amount. Reduce 0% at 1 resource. all the way to maxTimerReduction when nearbyResourceNodeAmount equal to maxResourceAmount, hence the -1
            float timerReduction = maxTimerReduction * ((float)(nearbyResourceNodeAmount - 1) / (maxResourceAmount - 1));

            generateResouseTimerMax = resourceGeneratorData.generateResouseTimerMax * (1 - timerReduction);
        }
        Debug.Log(nearbyResourceNodeAmount);
        Debug.Log(generateResouseTimerMax);
    }

    private void Update() {
        generateResouseTimer -= Time.deltaTime;

        if (generateResouseTimer <= 0) {
            generateResouseTimer = generateResouseTimerMax;

            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceTypeSO, 1);
        }
    }
}
