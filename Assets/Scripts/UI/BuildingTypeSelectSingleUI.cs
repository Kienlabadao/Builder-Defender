using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuildingTypeSelectSingleUI : MonoBehaviour {
    [SerializeField] private Image iconImage;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private float spriteDefaultScale = 1f;
    [SerializeField] private float arrowSpriteScale = .6f;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();

        selectedImage.enabled = false;
    }

    public void SetBuildingTypeSO(BuildingTypeSO buildingTypeSO) {
        iconImage.sprite = buildingTypeSO.sprite;

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
}
