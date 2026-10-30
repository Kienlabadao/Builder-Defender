using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private ResourceTypeListSO resourceTypeListSO;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one ResourceManager instance!");
        }
        Instance = this;

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeListSO.resourceTypeSOList) {
            resourceAmountDictionary[resourceTypeSO] = 0;
        }
    }

    private void TestLogResourceAmountDictionary() {
        foreach (ResourceTypeSO resourceTypeSO in resourceAmountDictionary.Keys) {
            Debug.Log($"{resourceTypeSO.resourseName}: {resourceAmountDictionary[resourceTypeSO]}");
        }
    }

    public void AddResource(ResourceTypeSO resourceTypeSO, int amount) {
        resourceAmountDictionary[resourceTypeSO] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceTypeSO) {
        return resourceAmountDictionary[resourceTypeSO];
    }
}
