using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorEfficiencyUI : MonoBehaviour {
    [SerializeField] private BuildingGhost buildingGhost;
    [SerializeField] private Image resourceIcon;
    [SerializeField] private TextMeshProUGUI efficiencyText;

    private ResourceGeneratorData resourceGeneratorData;

    private void Update() {
        efficiencyText.text = $"{ResourceGenerator.GetEfficiencyPercentage(resourceGeneratorData, buildingGhost.transform.position)}%";
    }

    public void Show(ResourceGeneratorData resourceGeneratorData) {
        this.resourceGeneratorData = resourceGeneratorData;

        resourceIcon.sprite = resourceGeneratorData.resourceSO.sprite;

        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
