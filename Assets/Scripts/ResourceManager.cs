using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private ResourceListSO resourceListSO;
    [SerializeField] private ResourceAmountData[] startingResourceArray;

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
}
