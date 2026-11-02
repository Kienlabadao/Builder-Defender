using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class TooltipUI : MonoBehaviour {
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private TooltipResourceCostUI tooltipResourceCostUI;
    [SerializeField] private float tooltipTimerMax = 5f;
    [SerializeField] private Vector2 padding;
    [SerializeField] private float spacing;

    private RectTransform rectTransform;
    private Vector2 tooltipTextSize;
    private bool isUsingResourceCost = false;
    private float tooltipTimer;
    private bool isTimerActive = false;

    private void OnValidate() {
        padding.x = Mathf.Max(0f, padding.x);
        padding.y = Mathf.Max(0f, padding.y);
        spacing = Mathf.Max(0f, spacing);
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one TooltipUI instance!");
        }
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        Hide();
    }

    private void Update() {
        HandleFollowMouse();

        if (isTimerActive) {
            tooltipTimer -= Time.deltaTime;

            if (tooltipTimer <= 0f) {
                isTimerActive = false;

                Hide();
            }
        }
    }

    private void HandleFollowMouse() {
        Vector2 anchoredPosition = Mouse.current.position.ReadValue() / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetTooltipText(string text) {
        tooltipText.SetText(text);

        tooltipTextSize = tooltipText.GetPreferredValues();

        tooltipText.rectTransform.anchoredPosition = new Vector2(padding.x, -padding.y);
    }

    private void SetTooltipBackground() {
        Vector2 tooltipResourceCostSize = tooltipResourceCostUI.GetContentSize();
        float rectTransformWidth;
        float rectTransformHeight;

        if (isUsingResourceCost) {
            rectTransformWidth = tooltipResourceCostSize.x > tooltipTextSize.x ? tooltipResourceCostSize.x : tooltipTextSize.x;
            rectTransformHeight = tooltipResourceCostSize.y + tooltipTextSize.y + spacing;
        } else {
            rectTransformWidth = tooltipTextSize.x;
            rectTransformHeight = tooltipTextSize.y;
        }

        rectTransform.sizeDelta = new Vector2(rectTransformWidth, rectTransformHeight) + (padding * 2);
    }

    private void SetTooltip(string tooltipText, ResourceAmountData[] resourceAmountDataArray, bool useTimer) {
        this.isUsingResourceCost = resourceAmountDataArray != null && resourceAmountDataArray.Length > 0;
        isTimerActive = useTimer;
        if (isTimerActive) {
            tooltipTimer = tooltipTimerMax;
        }

        SetTooltipText(tooltipText);
        if (isUsingResourceCost) {
            tooltipResourceCostUI.Show(resourceAmountDataArray, new Vector2(padding.x, -(padding.y + tooltipTextSize.y + spacing)));
        } else {
            tooltipResourceCostUI.Hide();
        }
        SetTooltipBackground();

        HandleFollowMouse();

        gameObject.SetActive(true);
    }

    public void Show(string tooltipText, bool useTimer = false) {
        SetTooltip(tooltipText, null, useTimer);
    }

    public void Show(string tooltipText, ResourceAmountData[] resourceAmountDataArray, bool useTimer = false) {
        SetTooltip(tooltipText, resourceAmountDataArray, useTimer);
    }

    public void Hide() {
        isUsingResourceCost = false;
        isTimerActive = false;

        tooltipResourceCostUI.Hide();

        gameObject.SetActive(false);
    }
}