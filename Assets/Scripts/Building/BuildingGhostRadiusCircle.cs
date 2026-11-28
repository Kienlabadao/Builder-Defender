using UnityEngine;

public class BuildingGhostRadiusCircle : MonoBehaviour {
    [SerializeField] private BuildingGhost buildingGhost;
    [SerializeField] private RadiusCircle radiusCircle;

    private void Awake() {
        buildingGhost.OnBuildingGhostActive += BuildingGhost_OnBuildingGhostActive;
        buildingGhost.OnBuildingGhostInactive += BuildingGhost_OnBuildingGhostInactive;
    }

    private void Start() {
        radiusCircle.Hide();
    }

    private void OnDestroy() {
        buildingGhost.OnBuildingGhostActive -= BuildingGhost_OnBuildingGhostActive;
        buildingGhost.OnBuildingGhostInactive -= BuildingGhost_OnBuildingGhostInactive;
    }

    private void BuildingGhost_OnBuildingGhostInactive(object sender, System.EventArgs e) {
        radiusCircle.Hide();
    }

    private void BuildingGhost_OnBuildingGhostActive(object sender, BuildingGhost.OnBuildingGhostActiveEventArgs e) {
        SetRadiusCircle(e.buildingSO);
    }

    private void SetRadiusCircle(BuildingSO buildingSO) {
        if (buildingSO.canGenerateResource) {
            radiusCircle.Show(
                RadiusCircleType.HarvesterDetectionRange,
                buildingSO.resourceGeneratorData.resourceDetectionRadius
            );
        } else if (buildingSO.canDefend) {
            radiusCircle.Show(RadiusCircleType.AttackRange, buildingSO.defenseBuildingData.range);
        } else {
            radiusCircle.Hide();
        }
    }
}
