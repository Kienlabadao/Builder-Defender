using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorUI : MonoBehaviour {
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private ProgressUI progressUI;
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI amountGeneratedPerSecondText;

    private void Start() {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        resourceIcon.sprite = resourceGeneratorData.resourceSO.sprite;
        amountGeneratedPerSecondText.text = $"{resourceGenerator.GetAmountGeneratedPerSecond().ToString("0.#")}/s";
    }

    private void Update() {
        progressUI.SetProgressAmount(1 - resourceGenerator.GetGenerateResourceTimerNormalized());
    }
}
