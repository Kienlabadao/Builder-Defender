using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private SpriteRenderer ghostSpriteRenderer;
    [SerializeField] private GeneratorEfficiencyUI generatorEfficiencyUI;

    public event EventHandler<OnBuildingGhostActiveEventArgs> OnBuildingGhostActive;
    public class OnBuildingGhostActiveEventArgs {
        public BuildingSO buildingSO;
    }
    public event EventHandler OnBuildingGhostInactive;
    public event EventHandler OnValidPlacementPosition;
    public event EventHandler OnInvalidPlacementPosition;

    private BuildingSO buildingSO;
    private bool isBuildMode;
    private List<ResourceNode> previousNearbyResourceNodeList;

    private void Awake() {
        previousNearbyResourceNodeList = new List<ResourceNode>();
    }

    private void Start() {
        isBuildMode = BuildingManager.Instance.IsBuildModeActive();

        BuildingManager.Instance.OnBuildModeActive += BuildingManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive += BuildManager_OnBuildModeInactive;

        Hide();
    }

    private void Update() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

        transform.position = mouseWorldPosition;

        if (isBuildMode) {
            ValidateSpawnPosition(mouseWorldPosition);

            if (BuildingManager.Instance.IsActiveBuildingHarvestable()) {
                HandleHighlightNearbyResourceNode(mouseWorldPosition);
            }
        }
    }

    private void OnDestroy() {
        BuildingManager.Instance.OnBuildModeActive -= BuildingManager_OnBuildModeActive;
        BuildingManager.Instance.OnBuildModeInactive -= BuildManager_OnBuildModeInactive;
    }

    private void BuildingManager_OnBuildModeActive(object sender, BuildingManager.OnBuildModeActiveEventArgs e) {
        Show(e.activeBuildingSO);
    }

    private void BuildManager_OnBuildModeInactive(object sender, System.EventArgs e) {
        Hide();
    }

    private void ValidateSpawnPosition(Vector3 position) {
        if (BuildingManager.Instance.CanSpawnBuildingAtPosition(position, out string _)) {
            OnValidPlacementPosition?.Invoke(this, EventArgs.Empty);
        } else {
            OnInvalidPlacementPosition?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleHighlightNearbyResourceNode(Vector3 position) {
        ResourceGenerator.GetNearbyResourceNodeAmount(
                    BuildingManager.Instance.GetActiveBuildingResourceGeneratorData(),
                    position,
                    out List<ResourceNode> nearbyResourceNodeList
                );

        foreach (ResourceNode resourceNode in nearbyResourceNodeList) {
            if (previousNearbyResourceNodeList.Contains(resourceNode)) {
                previousNearbyResourceNodeList.Remove(resourceNode);
            } else {
                resourceNode.GetComponentInChildren<ResourceNodeVisual>().Highlight();
            }
        }

        foreach (ResourceNode resourceNode in previousNearbyResourceNodeList) {
            resourceNode.GetComponentInChildren<ResourceNodeVisual>().Unhighlight();
        }

        previousNearbyResourceNodeList = nearbyResourceNodeList;
    }

    private void Show(BuildingSO buildingSO) {
        isBuildMode = true;
        previousNearbyResourceNodeList = new List<ResourceNode>();
        this.buildingSO = buildingSO;
        ghostSpriteRenderer.sprite = buildingSO.sprite;

        if (buildingSO.canGenerateResource) {
            generatorEfficiencyUI.Show(buildingSO.resourceGeneratorData);
        } else {
            generatorEfficiencyUI.Hide();
        }

        ValidateSpawnPosition(UtilsClass.GetMouseWorldPosition());

        OnBuildingGhostActive?.Invoke(this, new OnBuildingGhostActiveEventArgs {
            buildingSO = buildingSO
        });

        ghostSpriteRenderer.gameObject.SetActive(true);
    }

    private void Hide() {
        isBuildMode = false;

        generatorEfficiencyUI.Hide();

        OnBuildingGhostInactive?.Invoke(this, EventArgs.Empty);

        ghostSpriteRenderer.gameObject.SetActive(false);
    }

    public BuildingSO GetBuildingSO() {
        return buildingSO;
    }
}
