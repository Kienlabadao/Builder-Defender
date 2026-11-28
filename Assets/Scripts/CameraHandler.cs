using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraHandler : MonoBehaviour {
    private enum ZoomDirection {
        Normal,
        Inverted
    }

    public event EventHandler OnInitializeCompleted;
    public event EventHandler<OnZoomEventArgs> OnZoom;
    public class OnZoomEventArgs : EventArgs {
        public float orthographicSize;
    }

    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float minOrthographicSize = 15f;
    [SerializeField] private float maxOrthographicSize = 40f;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private ZoomDirection zoomDirection = ZoomDirection.Normal;

    private CinemachineCamera cinemachineCamera;
    private Camera outputCamera;
    private float orthographicSize;
    private float targetOrthographicSize;

    private void Awake() {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start() {
        outputCamera = GetUnityCamera(cinemachineCamera);

        orthographicSize = cinemachineCamera.Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;

        OnInitializeCompleted?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement() {
        Vector2 inputVector = GameInputManager.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, inputVector.y, 0f);

        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void HandleZoom() {
        float zoomDelta = zoomDirection switch {
            ZoomDirection.Normal => -GameInputManager.Instance.GetZoomDelta(),
            ZoomDirection.Inverted => GameInputManager.Instance.GetZoomDelta(),
            _ => throw new NotImplementedException($"ZoomDirection {zoomDirection} not implemented!"),
        };

        targetOrthographicSize += zoomDelta * zoomAmount;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);

        cinemachineCamera.Lens.OrthographicSize = orthographicSize;

        if (cinemachineCamera.Lens.OrthographicSize != targetOrthographicSize) {
            OnZoom?.Invoke(this, new OnZoomEventArgs {
                orthographicSize = orthographicSize
            });
        }
    }

    private Camera GetUnityCamera(CinemachineCamera cinemachineCamera) {
        CinemachineBrain cinemachineBrain = CinemachineCore.FindPotentialTargetBrain(cinemachineCamera);

        if (cinemachineBrain != null) {
            return cinemachineBrain.OutputCamera;
        } else {
            Debug.LogError($"No matching CinemachineBrain found for '{cinemachineCamera.Name}'. Ensure an active CinemachineBrain exists in the scene with a matching Channel/Layer mask.", cinemachineCamera);

            return null;
        }
    }

    public float GetCameraAspectRatio() {
        return outputCamera.aspect;
    }

    public float GetCameraOrthographicSize() {
        return cinemachineCamera.Lens.OrthographicSize;
    }
}
