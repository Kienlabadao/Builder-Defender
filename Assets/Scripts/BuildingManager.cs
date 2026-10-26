using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour {
    [SerializeField] private BuildingTypeSO buildingTypeSO;

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Instantiate(buildingTypeSO.prefab, GetMouseWorldPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }
}
