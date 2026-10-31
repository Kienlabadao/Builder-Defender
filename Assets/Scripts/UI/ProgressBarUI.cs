using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private Image barImage;
    [SerializeField] private bool hideOnFilledAndNoFill = true;

    private void Awake() {
        barImage.fillAmount = 0f;
    }

    private void Start() {
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void SetProgressAmount(float progressNormalized) {
        barImage.fillAmount = progressNormalized;

        if (hideOnFilledAndNoFill && (progressNormalized == 0f || progressNormalized == 1f)) {
            Hide();
        } else {
            Show();
        }
    }
}
