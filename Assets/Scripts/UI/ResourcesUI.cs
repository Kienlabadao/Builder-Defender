using System.Collections.Generic;
using UnityEngine;

public class ResourcesUI : MonoBehaviour {
    [SerializeField] private ResourceListSO resourceSOList;
    [SerializeField] private Transform resourceTemplate;

    private Dictionary<ResourceSO, Transform> resourceTypeTransformDictionary;

    private void Awake() {
        resourceTypeTransformDictionary = new Dictionary<ResourceSO, Transform>();
    }

    private void Start() {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;

        foreach (ResourceSO resourceSO in resourceSOList.resourceSOList) {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            ResourcesSingleUI resourcesSingleUI = resourceTransform.GetComponent<ResourcesSingleUI>();

            resourceTransform.gameObject.SetActive(true);
            resourcesSingleUI.SetResourceSO(resourceSO);
            resourcesSingleUI.SetAmount(ResourceManager.Instance.GetResourceAmount(resourceSO));

            resourceTypeTransformDictionary[resourceSO] = resourceTransform;
        }

        resourceTemplate.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        ResourceManager.Instance.OnResourceAmountChanged -= ResourceManager_OnResourceAmountChanged;
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e) {
        foreach (ResourceSO resourceSO in resourceTypeTransformDictionary.Keys) {
            Transform resourceTransform = resourceTypeTransformDictionary[resourceSO];

            resourceTransform.GetComponent<ResourcesSingleUI>().SetAmount(ResourceManager.Instance.GetResourceAmount(resourceSO));
        }
    }
}
