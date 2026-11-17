using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {
    [SerializeField] private Building building;
    [SerializeField] private Button buildingDemolishButton;

    private void Awake() {
        if (building.CanBeDemolished()) {
            buildingDemolishButton.onClick.AddListener(() => {
                Destroy(building.gameObject);

                ResourceManager.Instance.RefundBuilding(building.GetBuildingSO());
            });
        }

        building.OnBuildingSelected += Building_OnBuildingSelected;
        building.OnBuildingDeselected += Building_OnBuildingDeselected;
    }

    private void Start() {
        HideBuildingDemolishButton();
    }

    private void OnDestroy() {
        building.OnBuildingSelected -= Building_OnBuildingSelected;
        building.OnBuildingDeselected -= Building_OnBuildingDeselected;
    }

    private void Building_OnBuildingDeselected(object sender, System.EventArgs e) {
        HideBuildingDemolishButton();
    }

    private void Building_OnBuildingSelected(object sender, System.EventArgs e) {
        ShowBuildingDemolishButton();
    }

    private void ShowBuildingDemolishButton() {
        if (building.CanBeDemolished()) {
            buildingDemolishButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishButton() {
        buildingDemolishButton.gameObject.SetActive(false);
    }
}
