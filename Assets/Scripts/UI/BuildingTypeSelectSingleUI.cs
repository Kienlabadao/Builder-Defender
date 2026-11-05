using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(MouseEnterExitEvents))]
public class BuildingTypeSelectSingleUI : MonoBehaviour {
    [SerializeField] private Image iconImage;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private float spriteDefaultScale = 1f;
    [SerializeField] private float arrowSpriteScale = .6f;

    private Button button;
    private MouseEnterExitEvents mouseEnterExitEvents;
    private Action onMouseEnterAction;
    private Action onMouseExitAction;

    private void Awake() {
        button = GetComponent<Button>();
        mouseEnterExitEvents = GetComponent<MouseEnterExitEvents>();

        selectedImage.enabled = false;
    }

    private void Start() {
        mouseEnterExitEvents.OnMouseEnter += MouseEnterExitEvents_OnMouseEnter;
        mouseEnterExitEvents.OnMouseExit += MouseEnterExitEvents_OnMouseExit;
    }

    private void OnDestroy() {
        mouseEnterExitEvents.OnMouseEnter += MouseEnterExitEvents_OnMouseEnter;
        mouseEnterExitEvents.OnMouseExit += MouseEnterExitEvents_OnMouseExit;
    }

    private void MouseEnterExitEvents_OnMouseExit(object sender, EventArgs e) {
        onMouseExitAction?.Invoke();
    }

    private void MouseEnterExitEvents_OnMouseEnter(object sender, EventArgs e) {
        onMouseEnterAction?.Invoke();
    }

    public void SetBuildingSO(BuildingSO buildingSO) {
        iconImage.sprite = buildingSO.sprite;

        iconImage.transform.localScale = new Vector3(spriteDefaultScale, spriteDefaultScale, spriteDefaultScale);
    }

    public void SetArrowButton() {
        iconImage.sprite = arrowSprite;

        iconImage.transform.localScale = new Vector3(arrowSpriteScale, arrowSpriteScale, arrowSpriteScale);
    }

    public void SetButtonOnClickAction(Action onClickAction) {
        button.onClick.AddListener(() => {
            onClickAction();
        });
    }

    public void SetSelected(bool isSelected) {
        selectedImage.enabled = isSelected;
    }

    public void SetOnMouseEnterAction(Action onMouseEnterAction) {
        this.onMouseEnterAction = onMouseEnterAction;
    }

    public void SetOnMouseExitAction(Action onMouseExitAction) {
        this.onMouseExitAction = onMouseExitAction;
    }
}
