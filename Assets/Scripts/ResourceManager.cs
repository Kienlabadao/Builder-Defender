using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    [SerializeField] private ResourceTypeListSO resourceTypeListSO;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    private void Awake() {
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
    }
}
