using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorEfficiencyUI : MonoBehaviour {
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI efficiencyText;

    private ResourceGeneratorData resourceGeneratorData;

    private void Update() {
        efficiencyText.text = $"{ResourceGenerator.GetEfficiencyPercentage(resourceGeneratorData, transform.position)}%";
    }

    public void Show(ResourceGeneratorData resourceGeneratorData) {
        this.resourceGeneratorData = resourceGeneratorData;

        resourceIcon.sprite = resourceGeneratorData.resourceTypeSO.sprite;

        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
