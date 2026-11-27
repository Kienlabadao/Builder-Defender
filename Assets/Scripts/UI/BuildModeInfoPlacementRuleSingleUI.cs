using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeInfoPlacementRuleSingleUI : MonoBehaviour {
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI ruleText;
    [SerializeField] private Sprite validIconSprite;
    [SerializeField] private Color validColor;
    [SerializeField] private Sprite invalidIconSprite;
    [SerializeField] private Color invalidColor;

    private void Awake() {
        SetInvalidVisual();
    }

    public void SetRuleText(string ruleText) {
        this.ruleText.text = ruleText;
    }

    public void SetValidVisual() {
        iconImage.sprite = validIconSprite;
        ruleText.color = validColor;
    }

    public void SetInvalidVisual() {
        iconImage.sprite = invalidIconSprite;
        ruleText.color = invalidColor;
    }
}
