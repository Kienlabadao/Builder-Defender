using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritePositionSortingOrder : MonoBehaviour {
    private const float PRECISION_MULTIPLIER = 5f;

    [SerializeField] private bool runOnce = true;
    [SerializeField] private float positionOffset;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate() {
        spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffset) * PRECISION_MULTIPLIER);

        if (runOnce) {
            enabled = false;
        }
    }
}
