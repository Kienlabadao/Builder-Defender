using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {
    [SerializeField] private Building building;
    [SerializeField] private Button buildingDemolishButton;
    [SerializeField] private BuildingRepairButtonUI buildingRepairButton;

    private bool isSelected = false;

    private void Awake() {
        if (building.CanBeDemolished()) {
            buildingDemolishButton.onClick.AddListener(() => {
                Destroy(building.gameObject);

                ResourceManager.Instance.RefundBuilding(building.GetBuildingSO());
            });
        }

        building.OnBuildingSelected += Building_OnBuildingSelected;
        building.OnBuildingDeselected += Building_OnBuildingDeselected;
        building.OnDamaged += Building_OnDamaged;
        building.OnHealed += Building_OnHealed;
    }

    private void Start() {
        buildingRepairButton.SetBuilding(building);

        HideBuildingDemolishButton();
        buildingRepairButton.Hide();
    }

    private void OnDestroy() {
        building.OnBuildingSelected -= Building_OnBuildingSelected;
        building.OnBuildingDeselected -= Building_OnBuildingDeselected;
        building.OnDamaged -= Building_OnDamaged;
        building.OnHealed -= Building_OnHealed;
    }

    private void Building_OnHealed(object sender, System.EventArgs e) {
        if (isSelected) {
            UpdateBuildingRepairButtonVisibility();
        }
    }

    private void Building_OnDamaged(object sender, System.EventArgs e) {
        if (isSelected) {
            UpdateBuildingRepairButtonVisibility();
        }
    }

    private void Building_OnBuildingDeselected(object sender, System.EventArgs e) {
        isSelected = false;

        HideBuildingDemolishButton();
        buildingRepairButton.Hide();
    }

    private void Building_OnBuildingSelected(object sender, System.EventArgs e) {
        isSelected = true;

        UpdateBuildingDemolishButtonVisibility();
        UpdateBuildingRepairButtonVisibility();
    }

    private void UpdateBuildingDemolishButtonVisibility() {
        if (building.CanBeDemolished()) {
            ShowBuildingDemolishButton();
        } else {
            HideBuildingDemolishButton();
        }
    }

    private void UpdateBuildingRepairButtonVisibility() {
        if (building.CanRepair()) {
            buildingRepairButton.Show();
        } else {
            buildingRepairButton.Hide();
        }
    }

    private void ShowBuildingDemolishButton() {
        buildingDemolishButton.gameObject.SetActive(true);
    }

    private void HideBuildingDemolishButton() {
        buildingDemolishButton.gameObject.SetActive(false);
    }
}
