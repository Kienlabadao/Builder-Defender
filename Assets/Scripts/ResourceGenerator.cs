using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class ResourceGenerator : MonoBehaviour {
    private const float MAX_TIMER_REDUCTION = .5f;

    public event EventHandler OnInitializeCompleted;
    public event EventHandler OnGeneratorStopped;

    private ResourceGeneratorData resourceGeneratorData;
    private float generateResourceTimer;
    private float generateResourceTimerMax;


    private void Awake() {
        resourceGeneratorData = GetComponent<Building>().GetBuildingSO().resourceGeneratorData;
    }

    private void Start() {
        int nearbyResourceNodeAmount = GetNearbyResourceNodeAmount(resourceGeneratorData, transform.position, out List<ResourceNode> _);

        if (nearbyResourceNodeAmount == 0) {
            enabled = false;
        } else {
            generateResourceTimerMax = GetGenerateResourceTimerMax(resourceGeneratorData, nearbyResourceNodeAmount);
            generateResourceTimer = generateResourceTimerMax;
        }

        OnInitializeCompleted?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        generateResourceTimer -= Time.deltaTime;

        if (generateResourceTimer <= 0) {
            generateResourceTimer = generateResourceTimerMax;

            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceSO, 1);
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
        return GetGeneratorAmountGeneratedPerSecond(generateResourceTimerMax);
    }

    public bool IsEnabled() {
        return enabled;
    }

    public static int GetNearbyResourceNodeAmount(
        ResourceGeneratorData resourceGeneratorData,
        Vector3 position,
        out List<ResourceNode> nearbyResourceNodeList
    ) {
        nearbyResourceNodeList = new List<ResourceNode>();
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(
            position,
            resourceGeneratorData.resourceDetectionRadius
        );
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;

        int nearbyResourceNodeAmount = 0;
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out ResourceNode resourceNode) && resourceNode.GetResourceSO() == resourceGeneratorData.resourceSO) {
                nearbyResourceNodeAmount++;
                nearbyResourceNodeList.Add(resourceNode);
            }
        }

        return Mathf.Clamp(nearbyResourceNodeAmount, 0, maxResourceAmount);
    }

    public static int GetEfficiencyPercentage(ResourceGeneratorData resourceGeneratorData, Vector3 position) {
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;
        int nearbyResourceNodeAmount = GetNearbyResourceNodeAmount(resourceGeneratorData, position, out List<ResourceNode> _);

        return Mathf.RoundToInt((float)nearbyResourceNodeAmount / maxResourceAmount * 100f);
    }

    public static float GetGenerateResourceTimerMax(
        ResourceGeneratorData resourceGeneratorData,
        int nearbyResourceNodeAmount
    ) {
        if (nearbyResourceNodeAmount <= 0) {
            return 0f;
        }

        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;

        // Calculate timer reduction percentage based on the resource amount. Reduce 0% at 1 resource. all the way to maxTimerReduction when nearbyResourceNodeAmount equal to maxResourceAmount, hence the -1
        float timerReduction = MAX_TIMER_REDUCTION * ((float)(nearbyResourceNodeAmount - 1) / (maxResourceAmount - 1));

        return resourceGeneratorData.generateResourceTimerMax * (1 - timerReduction);
    }

    public static float GetGeneratorAmountGeneratedPerSecond(float generateResourceTimerMax) {
        if (generateResourceTimerMax <= 0f) {
            return 0f;
        }

        return 1f / generateResourceTimerMax;
    }

    public static float GetGeneratorAmountGeneratedPerSecond(
        ResourceGeneratorData resourceGeneratorData,
        int nearbyResourceNodeAmount
    ) {
        return GetGeneratorAmountGeneratedPerSecond(GetGenerateResourceTimerMax(
            resourceGeneratorData,
            nearbyResourceNodeAmount
        ));
    }
}
