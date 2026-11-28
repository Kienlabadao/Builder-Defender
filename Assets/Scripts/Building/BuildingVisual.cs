using UnityEngine;

public class BuildingVisual : MonoBehaviour {
    [SerializeField] private Building building;
    [SerializeField] private SpriteRenderer[] spriteRendererArray;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private bool isHighlighted = false;
    private bool isSelected = false;

    private void Awake() {
        building.OnBuildingHovered += Building_OnBuildingHovered;
        building.OnBuildingUnhovered += Building_OnBuildingUnhovered;
        building.OnBuildingSelected += Building_OnBuildingSelected;
        building.OnBuildingDeselected += Building_OnBuildingDeselected;
    }

    private void Start() {
        Unhighlight();
    }

    private void OnDestroy() {
        building.OnBuildingHovered -= Building_OnBuildingHovered;
        building.OnBuildingUnhovered -= Building_OnBuildingUnhovered;
        building.OnBuildingSelected -= Building_OnBuildingSelected;
        building.OnBuildingDeselected -= Building_OnBuildingDeselected;
    }

    private void Building_OnBuildingDeselected(object sender, System.EventArgs e) {
        isSelected = false;

        SetHighlight(false);
    }

    private void Building_OnBuildingSelected(object sender, System.EventArgs e) {
        isSelected = true;

        SetHighlight(true);
    }

    private void Building_OnBuildingUnhovered(object sender, System.EventArgs e) {
        SetHighlight(false);
    }

    private void Building_OnBuildingHovered(object sender, System.EventArgs e) {
        SetHighlight(true);
    }

    private void SetHighlight(bool isHighlighted) {
        if (!this.isHighlighted && isHighlighted) {
            this.isHighlighted = true;

            Highlight();
        } else if (!isSelected && this.isHighlighted && !isHighlighted) {
            this.isHighlighted = false;

            Unhighlight();
        }
    }

    private void Highlight() {
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
            spriteRenderer.material = highlightMaterial;
        }
    }

    private void Unhighlight() {
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
            spriteRenderer.material = defaultMaterial;
        }
    }
}
