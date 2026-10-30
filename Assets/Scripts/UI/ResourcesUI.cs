using System.Collections.Generic;
using UnityEngine;

public class ResourcesUI : MonoBehaviour {
    [SerializeField] private ResourceTypeListSO resourceTypeSOList;
    [SerializeField] private Transform resourceTemplate;

    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

    private void Awake() {
        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        resourceTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeSOList.resourceTypeSOList) {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            ResourcesSingleUI resourcesSingleUI = resourceTransform.GetComponent<ResourcesSingleUI>();

            resourceTransform.gameObject.SetActive(true);
            resourcesSingleUI.SetResourceTypeSO(resourceTypeSO);
            resourcesSingleUI.SetAmount(ResourceManager.Instance.GetResourceAmount(resourceTypeSO));

            resourceTypeTransformDictionary[resourceTypeSO] = resourceTransform;
        }
    }

    private void OnDestroy() {
        ResourceManager.Instance.OnResourceAmountChanged -= ResourceManager_OnResourceAmountChanged;
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e) {
        foreach (ResourceTypeSO resourceTypeSO in resourceTypeTransformDictionary.Keys) {
            Transform resourceTransform = resourceTypeTransformDictionary[resourceTypeSO];

            resourceTransform.GetComponent<ResourcesSingleUI>().SetAmount(ResourceManager.Instance.GetResourceAmount(resourceTypeSO));
        }
    }
}
