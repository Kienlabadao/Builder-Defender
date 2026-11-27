using TMPro;
using UnityEngine;

public class BuildModeInfoUI : MonoBehaviour {
    [SerializeField] private BuildModeInfoPlacementRulesUI placementRulesUI;
    [SerializeField] private BuildModeInfoHarvesterEffectivenessUI harvesterEffectivenessUI;
    [SerializeField] private TextMeshProUGUI buildingNameText;

    private void Start() {
        BuildingManager.Instance.OnBuildModeActive += BuildingManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive += BuildingManager_OnBuildModeInactive;

        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        BuildingManager.Instance.OnBuildModeActive -= BuildingManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive -= BuildingManager_OnBuildModeInactive;
    }

    private void BuildingManager_OnBuildModeInactive(object sender, System.EventArgs e) {
        Hide();
    }

    private void BuildingManager_OnBuildModeActive(object sender, BuildingManager.OnBuildModeActiveEventArgs e) {
        Show(e.activeBuildingSO);
    }

    private void Show(BuildingSO buildingSO) {
        buildingNameText.text = buildingSO.buildingName;
        placementRulesUI.Show();

        if (buildingSO.canGenerateResource) {
            harvesterEffectivenessUI.Show(buildingSO.resourceGeneratorData);
        } else {
            harvesterEffectivenessUI.Hide();
        }

        gameObject.SetActive(true);
    }

    private void Hide() {
        placementRulesUI.Hide();
        harvesterEffectivenessUI.Hide();

        gameObject.SetActive(false);
    }
}
