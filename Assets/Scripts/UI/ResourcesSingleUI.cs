using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesSingleUI : MonoBehaviour {
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetResourceSO(ResourceSO resourceSO) {
        iconImage.sprite = resourceSO.sprite;
    }

    public void SetAmount(int amount) {
        amountText.text = amount.ToString();
    }
}
