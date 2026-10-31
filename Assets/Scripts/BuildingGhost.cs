using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private SpriteRenderer ghostSpriteRenderer;
    [SerializeField] private GeneratorEfficiencyUI generatorEfficiencyUI;

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;

        Hide();
    }

    private void Update() {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void OnDestroy() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged -= BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        if (e.activeBuildingTypeSO) {
            Show(e.activeBuildingTypeSO);
        } else {
            Hide();
        }
    }

    private void Show(BuildingTypeSO buildingTypeSO) {
        ghostSpriteRenderer.sprite = buildingTypeSO.sprite;
        generatorEfficiencyUI.Show(buildingTypeSO.resourceGeneratorData);

        ghostSpriteRenderer.gameObject.SetActive(true);
    }

    private void Hide() {
        generatorEfficiencyUI.Hide();

        ghostSpriteRenderer.gameObject.SetActive(false);
    }
}
