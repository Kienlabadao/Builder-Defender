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
        if (e.activeBuildingSO) {
            Show(e.activeBuildingSO);
        } else {
            Hide();
        }
    }

    private void Show(BuildingSO buildingSO) {
        ghostSpriteRenderer.sprite = buildingSO.sprite;

        if (buildingSO.canGenerateResource) {
            generatorEfficiencyUI.Show(buildingSO.resourceGeneratorData);
        } else {
            generatorEfficiencyUI.Hide();
        }

        ghostSpriteRenderer.gameObject.SetActive(true);
    }

    private void Hide() {
        generatorEfficiencyUI.Hide();

        ghostSpriteRenderer.gameObject.SetActive(false);
    }
}
