using UnityEngine;

public class ResourceNode : MonoBehaviour {
    [SerializeField] private ResourceSO resourceSO;

    public ResourceSO GetResourceSO() {
        return resourceSO;
    }
}
