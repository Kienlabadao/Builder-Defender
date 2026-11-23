using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraMinimapIcon : MonoBehaviour {
    [SerializeField] private CameraHandler cameraHandler;

    private SpriteRenderer spriteRenderer;
    private Sprite minimapIconSprite;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        minimapIconSprite = spriteRenderer.sprite;

        cameraHandler.OnInitializeCompleted += CameraHandler_OnInitializeCompleted;
        cameraHandler.OnZoom += CameraHandler_OnZoom;
    }

    private void OnDestroy() {
        cameraHandler.OnInitializeCompleted -= CameraHandler_OnInitializeCompleted;
    }

    private void CameraHandler_OnZoom(object sender, CameraHandler.OnZoomEventArgs e) {
        SetTransformScale(e.orthographicSize);
    }

    private void CameraHandler_OnInitializeCompleted(object sender, EventArgs e) {
        SetTransformScale(cameraHandler.GetCameraOrthographicSize());
    }

    private void SetTransformScale(float orthographicSize) {
        float spriteOriginalWidth = minimapIconSprite.bounds.size.x;
        float spriteOriginalHeight = minimapIconSprite.bounds.size.y;

        float cameraViewHeight = orthographicSize * 2;
        float cameraViewWidth = cameraViewHeight * cameraHandler.GetCameraAspectRatio();
        float scaleX = cameraViewWidth / spriteOriginalWidth;
        float scaleY = cameraViewHeight / spriteOriginalHeight;

        transform.localScale = new Vector3(
            scaleX,
            scaleY,
            transform.localScale.z
        );
    }
}
