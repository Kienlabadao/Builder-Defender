using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    [Serializable]
    public struct RepairResourceCostData {
        public ResourceAmountData resourceAmountData;
        public float repairHPAmount;
    }

    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private ResourceListSO resourceListSO;
    [SerializeField] private ResourceAmountData[] startingResourceArray;
    [SerializeField] private float refundBuildingResourcePercentage = .6f;
    [SerializeField] private RepairResourceCostData[] repairResourceCostDataArray;

    private Dictionary<ResourceSO, int> resourceAmountDictionary;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one ResourceManager instance!");
        }
        Instance = this;

        resourceAmountDictionary = new Dictionary<ResourceSO, int>();

        foreach (ResourceSO resourceSO in resourceListSO.resourceSOArray) {
            resourceAmountDictionary[resourceSO] = 0;
        }

        foreach (ResourceAmountData resourceAmountData in startingResourceArray) {
            resourceAmountDictionary[resourceAmountData.resourceSO] = resourceAmountData.amount;
        }
    }

    private void TestLogResourceAmountDictionary() {
        foreach (ResourceSO resourceSO in resourceAmountDictionary.Keys) {
            Debug.Log($"{resourceSO.resourseName}: {resourceAmountDictionary[resourceSO]}");
        }
    }

    public bool CanAfford(ResourceAmountData[] resourceAmountDataArray) {
        foreach (ResourceAmountData resourceAmountData in resourceAmountDataArray) {
            if (resourceAmountDictionary[resourceAmountData.resourceSO] < resourceAmountData.amount) {
                return false;
            }
        }

        return true;
    }

    public void SpendResource(ResourceAmountData[] resourceAmountDataArray) {
        foreach (ResourceAmountData resourceAmountData in resourceAmountDataArray) {
            if (resourceAmountDictionary[resourceAmountData.resourceSO] < resourceAmountData.amount) {
                Debug.LogError($"Not enough resources! Resource: {resourceAmountData.resourceSO}. Amount currently: {resourceAmountDictionary[resourceAmountData.resourceSO]}. Amount needed: {resourceAmountData.amount}");
            }

            resourceAmountDictionary[resourceAmountData.resourceSO] -= resourceAmountData.amount;
        }

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddResource(ResourceSO resourceSO, int amount) {
        resourceAmountDictionary[resourceSO] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceSO resourceSO) {
        return resourceAmountDictionary[resourceSO];
    }

    public void RefundBuilding(BuildingSO buildingSO) {
        foreach (ResourceAmountData resourceAmountData in buildingSO.constructionResourceCostArray) {
            AddResource(
                resourceAmountData.resourceSO,
                Mathf.FloorToInt(resourceAmountData.amount * refundBuildingResourcePercentage)
            );
        }
    }

    public ResourceAmountData[] GetRepairResourceCost(float missingHealth) {
        List<ResourceAmountData> resourceAmountCost = new List<ResourceAmountData>();

        if (missingHealth > 0f) {
            foreach (RepairResourceCostData repairResourceCostData in repairResourceCostDataArray) {
                ResourceAmountData resourceAmountData = repairResourceCostData.resourceAmountData;
                int resourceRepairCost = Mathf.CeilToInt(
                    missingHealth / repairResourceCostData.repairHPAmount * resourceAmountData.amount
                );

                resourceAmountCost.Add(new ResourceAmountData {
                    resourceSO = resourceAmountData.resourceSO,
                    amount = resourceRepairCost
                });
            }
        }

        return resourceAmountCost.ToArray();
    }

    public bool CanAffordRepair(float missingHealth) {
        ResourceAmountData[] resourceAmountCost = GetRepairResourceCost(missingHealth);

        return CanAfford(resourceAmountCost);
    }

    public void SpendRepair(float missingHealth) {
        ResourceAmountData[] resourceAmountCost = GetRepairResourceCost(missingHealth);

        SpendResource(resourceAmountCost);
    }
}
