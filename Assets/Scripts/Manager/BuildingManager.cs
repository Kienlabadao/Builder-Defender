using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance { get; private set; }

    public enum BuildingPlacementError {
        NoAciveBuilding,
        CannotBePlaced,
        AreaNotClear,
        TooCloseToSameBuilding,
        TooFarFromAnotherBuilding
    }

    public event EventHandler<OnBuildModeActiveEventArgs> OnBuildModeActive;
    public class OnBuildModeActiveEventArgs {
        public BuildingSO activeBuildingSO;
    }
    public event EventHandler OnBuildModeInactive;
    public event EventHandler OnHQBuildingDestroyed;

    [SerializeField] private Transform buildingContainer;
    [SerializeField] private Building hqBuilding;
    [SerializeField] private float maxConstructionRadius = 25f;

    private BuildingSO activeBuildingSO;
    private bool isInteractInputRecieved = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one BuildingManager instance!");
        }
        Instance = this;

        hqBuilding.OnBuildingDestroyed += HQBuilding_OnBuildingDestroyed;
    }

    private void Start() {
        GameInputManager.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update() {
        if (isInteractInputRecieved) {
            isInteractInputRecieved = false;

            if (!EventSystem.current.IsPointerOverGameObject() &&
                activeBuildingSO != null) {
                if (ResourceManager.Instance.CanAfford(activeBuildingSO.constructionResourceCostArray)) {
                    Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

                    if (CanSpawnBuilding(activeBuildingSO,
                        mouseWorldPosition,
                        out List<BuildingPlacementError> buildingPlacementErrorList)
                    ) {
                        ResourceManager.Instance.SpendResource(activeBuildingSO.constructionResourceCostArray);

                        BuildingConstruction.SpawnBuildingConstruction(
                            mouseWorldPosition,
                            activeBuildingSO,
                            buildingContainer
                        );

                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    } else {
                        TooltipUI.Instance.Show(GetPlacementErrorMessage(buildingPlacementErrorList[0]), true);
                    }
                } else {
                    TooltipUI.Instance.Show("Cannot afford!", activeBuildingSO.constructionResourceCostArray, true);
                }
            }
        }
    }

    private void OnDestroy() {
        GameInputManager.Instance.OnInteractAction -= GameInput_OnInteractAction;
        hqBuilding.OnBuildingDestroyed -= HQBuilding_OnBuildingDestroyed;
    }

    private void HQBuilding_OnBuildingDestroyed(object sender, EventArgs e) {
        OnHQBuildingDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        isInteractInputRecieved = true;
    }

    private bool CanSpawnBuilding(
        BuildingSO buildingSO,
        Vector3 position,
        out List<BuildingPlacementError> buildingPlacementErrorList
    ) {
        buildingPlacementErrorList = new List<BuildingPlacementError>();

        if (!buildingSO.canBePlaced) {
            buildingPlacementErrorList.Add(BuildingPlacementError.CannotBePlaced);
            Debug.LogWarning($"Building {buildingSO.name} aren't allowed to be placed!");

            return false;
        }

        BoxCollider2D boxCollider2D = buildingSO.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0f);
        if (collider2DArray.Length != 0) {
            buildingPlacementErrorList.Add(BuildingPlacementError.AreaNotClear);
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingSO.minConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out Building building)) {
                if (building.GetBuildingSO() != buildingSO) {
                    continue;
                }
            } else if (collider2D.TryGetComponent(out BuildingConstruction buildingConstruction)) {
                if (buildingConstruction.GetBuildingSO() != buildingSO) {
                    continue;
                }
            } else {
                continue;
            }

            buildingPlacementErrorList.Add(BuildingPlacementError.TooCloseToSameBuilding);
            break;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        bool isBuildingNearby = false;
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out Building _)) {
                isBuildingNearby = true;

                break;
            }
        }

        if (!isBuildingNearby) {
            buildingPlacementErrorList.Add(BuildingPlacementError.TooFarFromAnotherBuilding);
        }

        return buildingPlacementErrorList.Count == 0;
    }

    public bool CanSpawnBuildingAtPosition(
        Vector3 position,
        out List<BuildingPlacementError> buildingPlacementErrorList
    ) {
        if (activeBuildingSO) {
            return CanSpawnBuilding(activeBuildingSO, position, out buildingPlacementErrorList);
        } else {
            buildingPlacementErrorList = new List<BuildingPlacementError> { BuildingPlacementError.NoAciveBuilding };

            return false;
        }
    }

    public void SetActiveBuildingSO(BuildingSO buildingSO) {
        if (buildingSO != null && !buildingSO.canBePlaced) {
            Debug.LogWarning($"Building {buildingSO.name} aren't allowed to be placed!");

            return;
        }

        activeBuildingSO = buildingSO;

        if (activeBuildingSO) {
            OnBuildModeActive?.Invoke(this, new OnBuildModeActiveEventArgs {
                activeBuildingSO = activeBuildingSO
            });
        } else {
            OnBuildModeInactive?.Invoke(this, EventArgs.Empty);
        }

    }

    public BuildingSO GetActiveBuildingSO() {
        return activeBuildingSO;
    }

    public Building GetHQBuilding() {
        return hqBuilding;
    }

    public bool IsBuildModeActive() {
        return activeBuildingSO != null;
    }

    public bool IsActiveBuildingHarvestable() {
        return activeBuildingSO != null && activeBuildingSO.canGenerateResource;
    }

    public ResourceGeneratorData GetActiveBuildingResourceGeneratorData() {
        if (IsActiveBuildingHarvestable()) {
            return activeBuildingSO.resourceGeneratorData;
        } else {
            return null;
        }
    }

    private static string GetPlacementErrorMessage(BuildingPlacementError error) {
        return error switch {
            BuildingPlacementError.NoAciveBuilding => "No currently active building!",
            BuildingPlacementError.CannotBePlaced => "This building cannot be placed!",
            BuildingPlacementError.AreaNotClear => "Area is not clear!",
            BuildingPlacementError.TooCloseToSameBuilding => "Too close to another building of the same type!",
            BuildingPlacementError.TooFarFromAnotherBuilding => "Too far from any other building!",
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}
