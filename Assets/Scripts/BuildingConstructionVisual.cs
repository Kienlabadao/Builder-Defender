using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingConstructionVisual : MonoBehaviour {
    private const string SHADER_ID_PROGRESS = "_Progress";

    [SerializeField] private BuildingConstruction buildingConstruction;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    private void Start() {
        spriteRenderer.sprite = buildingConstruction.GetBuildingSO().sprite;
    }

    private void Update() {
        material.SetFloat(SHADER_ID_PROGRESS, 1 - buildingConstruction.GetContructionTimerNormalized());
    }
}
