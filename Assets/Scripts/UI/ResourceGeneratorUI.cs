using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorUI : MonoBehaviour {
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI amountGeneratedPerSecondText;

    private void Start() {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        resourceIcon.sprite = resourceGeneratorData.resourceTypeSO.sprite;
        amountGeneratedPerSecondText.text = $"{resourceGenerator.GetAmountGeneratedPerSecond().ToString("0.#")}/s";
    }

    private void Update() {
        progressBarUI.SetProgressAmount(1 - resourceGenerator.GetGenerateResourceTimerNormalized());
    }
}
