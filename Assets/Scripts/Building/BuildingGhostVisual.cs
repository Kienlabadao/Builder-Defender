using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingGhostVisual : MonoBehaviour {
    private const string SHADER_ID_HIGHLIGHT_COLOR = "_HighlightColor";

    [SerializeField] private BuildingGhost buildingGhost;
    [SerializeField] private Color validPlacementColor;
    [SerializeField] private Color invalidPlacementColor;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        buildingGhost.OnValidPlacementPosition += BuildingGhost_OnValidPlacementPosition;
        buildingGhost.OnInvalidPlacementPosition += BuildingGhost_OnInvalidPlacementPosition;
    }

    private void OnDestroy() {
        buildingGhost.OnValidPlacementPosition -= BuildingGhost_OnValidPlacementPosition;
        buildingGhost.OnInvalidPlacementPosition -= BuildingGhost_OnInvalidPlacementPosition;
    }

    private void BuildingGhost_OnInvalidPlacementPosition(
        object sender,
        BuildingGhost.OnInvalidPlacementPositionEventArgs e
    ) {
        spriteRenderer.material.SetColor(SHADER_ID_HIGHLIGHT_COLOR, invalidPlacementColor);
    }

    private void BuildingGhost_OnValidPlacementPosition(object sender, System.EventArgs e) {
        spriteRenderer.material.SetColor(SHADER_ID_HIGHLIGHT_COLOR, validPlacementColor);
    }
}
