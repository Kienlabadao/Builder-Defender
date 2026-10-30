using UnityEngine;

public class ResourceNode : MonoBehaviour {
    [SerializeField] private ResourceTypeSO resourceTypeSO;

    public ResourceTypeSO GetResourceTypeSO() {
        return resourceTypeSO;
    }
}
