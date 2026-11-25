using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(MouseEnterExitEvents))]
public class BuildingRepairButtonUI : MonoBehaviour {
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightedColor;
    [SerializeField] private Color pressedColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color disabledColor;

    private Button button;
    private ColorBlock buttonColorBlock;
    private MouseEnterExitEvents mouseEnterExitEvents;
    private Building building;
    private Action onMouseEnterAction;
    private Action onMouseExitAction;
    private bool isMouseOnButton = false;
    private bool isButtonClickedRecently = false;

    private void Awake() {
        button = GetComponent<Button>();
        mouseEnterExitEvents = GetComponent<MouseEnterExitEvents>();

        buttonColorBlock = button.colors;

        mouseEnterExitEvents.OnMouseEnter += MouseEnterExitEvents_OnMouseEnter;
        mouseEnterExitEvents.OnMouseExit += MouseEnterExitEvents_OnMouseExit;
    }

    private void Update() {
        SetButtonVisual();

        if (isMouseOnButton && !isButtonClickedRecently) {
            UpdateBuildingCostTooltip();
        }
    }

    private void OnDestroy() {
        if (isMouseOnButton) {
            TooltipUI.Instance.Hide();
        }

        mouseEnterExitEvents.OnMouseEnter -= MouseEnterExitEvents_OnMouseEnter;
        mouseEnterExitEvents.OnMouseExit -= MouseEnterExitEvents_OnMouseExit;
    }

    private void MouseEnterExitEvents_OnMouseExit(object sender, EventArgs e) {
        HandleMouseExit();
    }

    private void MouseEnterExitEvents_OnMouseEnter(object sender, EventArgs e) {
        HandleMouseEnter();
    }

    private void HandleMouseEnter() {
        isMouseOnButton = true;
        isButtonClickedRecently = false;

        onMouseEnterAction?.Invoke();
    }

    private void HandleMouseExit() {
        isMouseOnButton = false;
        isButtonClickedRecently = false;

        onMouseExitAction?.Invoke();
    }

    private void SetButtonOnClickAction() {
        button.onClick.AddListener(() => {
            float buildingMissingHealth = building.GetMissingHealthAmount();

            if (ResourceManager.Instance.CanAffordRepair(buildingMissingHealth)) {
                building.HealFull();

                ResourceManager.Instance.SpendRepair(buildingMissingHealth);
            } else {
                isButtonClickedRecently = true;

                TooltipUI.Instance.Show("Cannot afford repair cost!", ResourceManager.Instance.GetRepairResourceCost(buildingMissingHealth), true);
            }
        });
    }

    private void AddButtonMouseActions() {
        onMouseEnterAction = () => {
            UpdateBuildingCostTooltip();
        };

        onMouseExitAction = () => {
            TooltipUI.Instance.Hide();
        };
    }

    private void UpdateBuildingCostTooltip() {
        float buildingMissingHealth = building.GetMissingHealthAmount();

        TooltipUI.Instance.Show("Repair Cost:", ResourceManager.Instance.GetRepairResourceCost(buildingMissingHealth));
    }

    private void SetButtonVisual() {
        float buildingMissingHealth = building.GetMissingHealthAmount();

        bool canRepair = building.CanRepair()
            && ResourceManager.Instance.CanAffordRepair(buildingMissingHealth);

        buttonColorBlock.normalColor = canRepair ? normalColor : disabledColor;
        buttonColorBlock.highlightedColor = canRepair ? highlightedColor : disabledColor;
        buttonColorBlock.pressedColor = canRepair ? pressedColor : disabledColor;
        buttonColorBlock.selectedColor = canRepair ? selectedColor : disabledColor;

        button.colors = buttonColorBlock;
    }

    public void SetBuilding(Building building) {
        this.building = building;

        SetButtonOnClickAction();
        AddButtonMouseActions();
    }

    public void Show() {
        SetButtonVisual();

        gameObject.SetActive(true);
    }

    public void Hide() {
        HandleMouseExit();

        gameObject.SetActive(false);
    }
}
