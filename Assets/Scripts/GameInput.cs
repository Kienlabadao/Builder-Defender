using System;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;

    private InputActions inputActions;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one GameInput instance!");
        }
        Instance = this;

        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
    }

    private void OnDestroy() {
        inputActions.Player.Interact.performed -= Interact_performed;

        inputActions.Dispose();
        inputActions.Disable();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public float GetZoomDelta() {
        return inputActions.Player.Zoom.ReadValue<Vector2>().y;
    }
}
