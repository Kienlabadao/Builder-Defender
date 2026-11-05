using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(RectTransform))]
public class TooltipResourceCostUI : MonoBehaviour {
    [SerializeField] private Transform resourceCostTemplate;

    private RectTransform rectTransform;
    private GridLayoutGroup gridLayoutGroup;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start() {
        resourceCostTemplate.gameObject.SetActive(false);
    }

    private void SetTooltip(ResourceAmountData[] resourceAmountDataArray, Vector2 rectTransformPosition) {
        ClearAllChilds();

        foreach (ResourceAmountData resourceAmountData in resourceAmountDataArray) {
            Transform resourceCostTransform = Instantiate(resourceCostTemplate, transform);
            TooltipResourceCostSingleUI tooltipResourceCostSingleUI = resourceCostTransform.GetComponent<TooltipResourceCostSingleUI>();

            resourceCostTransform.gameObject.SetActive(true);
            tooltipResourceCostSingleUI.SetResourceSO(resourceAmountData.resourceSO);
            tooltipResourceCostSingleUI.SetAmount(resourceAmountData.amount);
        }

        rectTransform.anchoredPosition = rectTransformPosition;
    }

    public void Show(ResourceAmountData[] resourceAmountDataArray, Vector2 rectTransformSize) {
        SetTooltip(resourceAmountDataArray, rectTransformSize);

        gameObject.SetActive(true);
    }

    public void Hide() {
        ClearAllChilds();

        gameObject.SetActive(false);
    }

    private void ClearAllChilds() {
        foreach (Transform child in transform) {
            if (child != resourceCostTemplate) {
                child.gameObject.SetActive(false);

                Destroy(child.gameObject);
            }
        }
    }

    public Vector2 GetContentSize() {
        return UtilsClass.GetGridLayoutGroupContentSize(gridLayoutGroup);
    }
}
