using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorUI : MonoBehaviour {
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private ProgressUI progressUI;
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI amountGeneratedPerSecondText;

    private void Awake() {
        resourceGenerator.OnInitializeCompleted += ResourceGenerator_OnInitializeCompleted;
    }

    private void Update() {
        progressUI.SetProgressAmount(1 - resourceGenerator.GetGenerateResourceTimerNormalized());
    }

    private void OnDestroy() {
        resourceGenerator.OnInitializeCompleted -= ResourceGenerator_OnInitializeCompleted;
    }

    private void ResourceGenerator_OnInitializeCompleted(object sender, System.EventArgs e) {
        Initialize();
    }

    private void Initialize() {
        if (resourceGenerator.IsEnabled()) {
            ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

            resourceIcon.sprite = resourceGeneratorData.resourceSO.sprite;
            amountGeneratedPerSecondText.text = $"{resourceGenerator.GetAmountGeneratedPerSecond().ToString("0.#")}/s";

            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
