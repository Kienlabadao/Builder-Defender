using UnityEngine;

public class BuildingConstructionRadiusCircle : MonoBehaviour {
    [SerializeField] private BuildingConstruction buildingConstruction;
    [SerializeField] private RadiusCircle radiusCircle;

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
        BuildingManager.Instance.OnBuildModeActive -= BuildManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive -= BuildingManager_OnBuildModeInactive;
    }

    private void BuildingManager_OnBuildModeInactive(object sender, System.EventArgs e) {
        radiusCircle.Hide();
    }

    private void BuildManager_OnBuildModeActive(object sender, BuildingManager.OnBuildModeActiveEventArgs e) {
        SetBuildModeRadiusCircle(e.activeBuildingSO);
    }

    private void SetBuildModeRadiusCircle(BuildingSO activeBuildingSO) {
        if (activeBuildingSO == buildingConstruction.GetBuildingSO()) {
            radiusCircle.Show(RadiusCircleType.MinimumBuildingDistance, activeBuildingSO.minConstructionRadius);
        } else {
            radiusCircle.Hide();
        }
    }
}
