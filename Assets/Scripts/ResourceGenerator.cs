using UnityEngine;

[RequireComponent(typeof(ResourceBuilding))]
public class ResourceGenerator : MonoBehaviour {
    private const float MAX_TIMER_REDUCTION = .5f;

    private ResourceGeneratorData resourceGeneratorData;
    private float generateResourceTimer;
    private float generateResourceTimerMax;


    private void Awake() {
        resourceGeneratorData = GetComponent<ResourceBuilding>().GetBuildingTypeSO().resourceGeneratorData;
    }

    private void Start() {
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;
        int nearbyResourceNodeAmount = GetNearbyResourceNodeAmount(resourceGeneratorData, transform.position);

        if (nearbyResourceNodeAmount == 0) {
            enabled = false;
        } else {
            // Calculate timer reduction percentage based on the resource amount. Reduce 0% at 1 resource. all the way to maxTimerReduction when nearbyResourceNodeAmount equal to maxResourceAmount, hence the -1
            float timerReduction = MAX_TIMER_REDUCTION * ((float)(nearbyResourceNodeAmount - 1) / (maxResourceAmount - 1));

            generateResourceTimerMax = resourceGeneratorData.generateResourceTimerMax * (1 - timerReduction);
        }
    }

    private void Update() {
        generateResourceTimer -= Time.deltaTime;

        if (generateResourceTimer <= 0) {
            generateResourceTimer = generateResourceTimerMax;

            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceTypeSO, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData() {
        return resourceGeneratorData;
    }

    public float GetGenerateResourceTimerNormalized() {
        if (generateResourceTimerMax <= 0f) {
            return 0f;
        }

        return generateResourceTimer / generateResourceTimerMax;
    }

    public float GetAmountGeneratedPerSecond() {
        if (generateResourceTimerMax <= 0f) {
            return 0f;
        }

        return 1f / generateResourceTimerMax;
    }

    public static int GetNearbyResourceNodeAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position) {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;

        int nearbyResourceNodeAmount = 0;
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out ResourceNode resourceNode) && resourceNode.GetResourceTypeSO() == resourceGeneratorData.resourceTypeSO) {
                nearbyResourceNodeAmount++;
            }
        }

        return Mathf.Clamp(nearbyResourceNodeAmount, 0, maxResourceAmount);
    }

    public static int GetEfficiencyPercentage(ResourceGeneratorData resourceGeneratorData, Vector3 position) {
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;
        int nearbyResourceNodeAmount = GetNearbyResourceNodeAmount(resourceGeneratorData, position);

        return Mathf.RoundToInt((float)nearbyResourceNodeAmount / maxResourceAmount * 100f);
    }
}
