using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs {
        public BuildingTypeSO activeBuildingTypeSO;
    }

    [SerializeField] private float maxConstructionRadius = 25f;

    private BuildingTypeSO activeBuildingTypeSO;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one BuildingManager instance!");
        }
        Instance = this;
    }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject()) {
            if (activeBuildingTypeSO != null &&
                CanSpawnBuilding(activeBuildingTypeSO, UtilsClass.GetMouseWorldPosition())) {
                Instantiate(activeBuildingTypeSO.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
            }
        }
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingTypeSO, Vector3 position) {
        if (!buildingTypeSO.canBePlaced) {
            Debug.LogWarning($"Building {buildingTypeSO.name} aren't allow to be placed!");

            return false;
        }

        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0f);
        if (collider2DArray.Length != 0) {
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingTypeSO.minConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out ResourceBuilding resourceBuilding) &&
                resourceBuilding.GetBuildingTypeSO() == buildingTypeSO) {
                return false;
            }
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out ResourceBuilding _)) {
                return true;
            }
        }

        return false;
    }

    public void SetActiveBuildingTypeSO(BuildingTypeSO buildingTypeSO) {
        activeBuildingTypeSO = buildingTypeSO;

        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs {
            activeBuildingTypeSO = activeBuildingTypeSO
        });
    }

    public BuildingTypeSO GetActiveBuildingTypeSO() {
        return activeBuildingTypeSO;
    }
}
