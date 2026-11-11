using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour {
    [SerializeField] private Image fillImage;
    [SerializeField] private bool hideOnFilledAndNoFill = true;

    private void Awake() {
        fillImage.fillAmount = 0f;
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
        fillImage.fillAmount = progressNormalized;

        if (hideOnFilledAndNoFill && (progressNormalized == 0f || progressNormalized == 1f)) {
            Hide();
        } else {
            Show();
        }
    }
}
