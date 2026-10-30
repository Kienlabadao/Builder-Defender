using UnityEngine;
using UnityEngine.InputSystem;

public static class UtilsClass {
    public static Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }
}
