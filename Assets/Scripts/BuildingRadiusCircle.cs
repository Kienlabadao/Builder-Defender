using UnityEngine;

public class BuildingRadiusCircle : MonoBehaviour {
    [SerializeField] private Building building;
    [SerializeField] private RadiusCircle radiusCircle;

    private void Awake() {
        building.OnBuildingSelected += Building_OnBuildingSelected;
        building.OnBuildingDeselected += Building_OnBuildingDeselected;
    }

    private void Start() {
        BuildingManager.Instance.OnBuildModeActive += BuildManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive += BuildingManager_OnBuildModeInactive;

        if (BuildingManager.Instance.IsBuildModeActive()) {
            SetBuildModeRadiusCircle(BuildingManager.Instance.GetActiveBuildingSO());
        } else {
            radiusCircle.Hide();
        }
    }

    private void OnDestroy() {
        building.OnBuildingSelected -= Building_OnBuildingSelected;
        building.OnBuildingDeselected -= Building_OnBuildingDeselected;
        BuildingManager.Instance.OnBuildModeActive -= BuildManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive -= BuildingManager_OnBuildModeInactive;
    }

    private void Building_OnBuildingDeselected(object sender, System.EventArgs e) {
        radiusCircle.Hide();
    }

    private void Building_OnBuildingSelected(object sender, System.EventArgs e) {
        SetSelectedRadiusCircle();
    }

    private void BuildingManager_OnBuildModeInactive(object sender, System.EventArgs e) {
        radiusCircle.Hide();
    }

    private void BuildManager_OnBuildModeActive(object sender, BuildingManager.OnBuildModeActiveEventArgs e) {
        SetBuildModeRadiusCircle(e.activeBuildingSO);
    }

    private void SetBuildModeRadiusCircle(BuildingSO activeBuildingSO) {
        if (activeBuildingSO == building.GetBuildingSO()) {
            radiusCircle.Show(RadiusCircleType.MinimumBuildingDistance, activeBuildingSO.minConstructionRadius);
        } else {
            radiusCircle.Hide();
        }
    }

    private void SetSelectedRadiusCircle() {
        BuildingSO buildingSO = building.GetBuildingSO();

        if (buildingSO.canDefend) {
            radiusCircle.Show(RadiusCircleType.AttackRange, buildingSO.defenseBuildingData.range);
        } else {
            radiusCircle.Hide();
        }
    }
}
