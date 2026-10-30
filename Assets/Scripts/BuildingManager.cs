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

    [SerializeField] private BuildingTypeListSO buildingTypeSOList;

    private BuildingTypeSO activeBuildingTypeSO;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one BuildingManager instance!");
        }
        Instance = this;
    }

    private void Update() {
        if (activeBuildingTypeSO != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject()) {
            Instantiate(activeBuildingTypeSO.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        }
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
