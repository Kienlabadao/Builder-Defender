using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeInfoHarvesterEffectivenessUI : MonoBehaviour {
    [SerializeField] private Image resourceIconImage;
    [SerializeField] private TextMeshProUGUI generateSpeedText;
    [SerializeField] private TextMeshProUGUI resourceNodeCountText;
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color noResourceNodeNearbyTextColor;
    [SerializeField] private Color resourceNodeNearbyTextColor;
    [SerializeField] private Color maxResourceNodeTextColor;

    private ResourceGeneratorData resourceGeneratorData;

    private void Start() {
        Hide();
    }

    private void Update() {
        int nearbyResourceNodeAmount = BuildingGhost.Instance.GetNearbyResourceNodeAmount();
        int maxResourceAmount = resourceGeneratorData.maxResourceAmount;
        float generateSpeed = ResourceGenerator.GetGeneratorAmountGeneratedPerSecond(resourceGeneratorData, nearbyResourceNodeAmount);

        generateSpeedText.text = $"{generateSpeed.ToString("0.#")}/s";
        resourceNodeCountText.text = $"{nearbyResourceNodeAmount}/{maxResourceAmount}";

        if (nearbyResourceNodeAmount == 0) {
            resourceNodeCountText.color = noResourceNodeNearbyTextColor;
            generateSpeedText.color = noResourceNodeNearbyTextColor;
        } else if (nearbyResourceNodeAmount >= maxResourceAmount) {
            resourceNodeCountText.color = maxResourceNodeTextColor;
            generateSpeedText.color = maxResourceNodeTextColor;
        } else {
            resourceNodeCountText.color = resourceNodeNearbyTextColor;
            generateSpeedText.color = defaultTextColor;
        }
    }

    public void Show(ResourceGeneratorData resourceGeneratorData) {
        this.resourceGeneratorData = resourceGeneratorData;

        resourceIconImage.sprite = resourceGeneratorData.resourceSO.sprite;

        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
