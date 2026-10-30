using UnityEngine;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one GameInput instance!");
        }
        Instance = this;

        inputActions = new InputActions();
        inputActions.Enable();
    }

    private void OnDestroy() {
        inputActions.Dispose();
        inputActions.Disable();
    }

    public Vector2 GetMovementVectorNormalized() {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public float GetZoomDelta() {
        return inputActions.Player.Zoom.ReadValue<Vector2>().y;
    }
}
