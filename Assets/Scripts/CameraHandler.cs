using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    private enum ZoomDirection {
        Normal,
        Inverted
    }

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minOrthographicSize = 10f;
    [SerializeField] private float maxOrthographicSize = 30f;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private ZoomDirection zoomDirection = ZoomDirection.Normal;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start() {
        orthographicSize = cinemachineCamera.Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, inputVector.y, 0f);

        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void HandleZoom() {
        float zoomDelta;
        switch (zoomDirection) {
            case ZoomDirection.Normal:
                zoomDelta = -GameInput.Instance.GetZoomDelta();

                break;
            case ZoomDirection.Inverted:
                zoomDelta = GameInput.Instance.GetZoomDelta();

                break;
            default:
                throw new NotImplementedException($"ZoomDirection {zoomDirection} not implemented!");
        }

        targetOrthographicSize += zoomDelta * zoomAmount;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);

        cinemachineCamera.Lens.OrthographicSize = orthographicSize;
    }
}
