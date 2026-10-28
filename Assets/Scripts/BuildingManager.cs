using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour {
    [SerializeField] private BuildingTypeListSO buildingTypeSOList;

    private BuildingTypeSO buildingTypeSO;

    private void Awake() {
        buildingTypeSO = buildingTypeSOList.buildingTypeSOList[0];
    }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Instantiate(buildingTypeSO.prefab, GetMouseWorldPosition(), Quaternion.identity);
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame) {
            buildingTypeSO = buildingTypeSOList.buildingTypeSOList[0];
        } else if (Keyboard.current.digit2Key.wasPressedThisFrame) {
            buildingTypeSO = buildingTypeSOList.buildingTypeSOList[1];
        } else if (Keyboard.current.digit3Key.wasPressedThisFrame) {
            buildingTypeSO = buildingTypeSOList.buildingTypeSOList[2];
        }
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }
}
