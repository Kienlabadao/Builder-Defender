using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {
    public static SelectionManager Instance { get; private set; }

    [SerializeField] private LayerMask selectionLayerMask;

    private ISelectable hoveredObject = null;
    private ISelectable selectedObject = null;

    private bool isInteractInputRecieved = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one SelectionManager instance!");
        }
        Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void Update() {
        HandleHovering();

        if (isInteractInputRecieved) {
            isInteractInputRecieved = false;

            if (!EventSystem.current.IsPointerOverGameObject()) {
                HandleHovering();
                HandleSelection();
            }
        }
    }

    private void OnDestroy() {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        BuildingManager.Instance.OnActiveBuildingTypeChanged -= BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        if (BuildingManager.Instance.IsInBuildMode()) {
            SetHoveredObject(null);
            HandleSelection();
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        isInteractInputRecieved = true;
    }

    private void HandleHovering() {
        if (!BuildingManager.Instance.IsInBuildMode()) {
            Collider2D collider2D = Physics2D.OverlapPoint(
                UtilsClass.GetMouseWorldPosition(),
                selectionLayerMask
            );

            if (collider2D != null) {
                SetHoveredObject(collider2D.GetComponentInParent<ISelectable>());
            } else {
                SetHoveredObject(null);
            }
        } else {
            SetHoveredObject(null);
        }
    }

    private void SetHoveredObject(ISelectable hoveredObject) {
        if (hoveredObject != null) {
            if (this.hoveredObject == null) {
                this.hoveredObject = hoveredObject;

                this.hoveredObject.Hover();
            } else {
                if (this.hoveredObject != hoveredObject) {
                    this.hoveredObject.Unhover();

                    this.hoveredObject = hoveredObject;

                    this.hoveredObject.Hover();
                }
            }
        } else {
            this.hoveredObject?.Unhover();

            this.hoveredObject = null;
        }
    }

    private void HandleSelection() {
        if (hoveredObject != null) {
            if (selectedObject == null) {
                selectedObject = hoveredObject;

                selectedObject.Select();
            } else {
                if (selectedObject != hoveredObject) {
                    selectedObject.Deselect();

                    selectedObject = hoveredObject;

                    selectedObject.Select();
                }
            }
        } else {
            selectedObject?.Deselect();

            selectedObject = null;
        }
    }
}
