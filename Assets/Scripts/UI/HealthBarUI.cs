using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image barImage;

    private void Start() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateBar();
    }

    private void OnDestroy() {
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        UpdateBar();
    }

    private void UpdateBar() {
        barImage.fillAmount = healthSystem.GetHealthAmountNormalized();

        UpdateHealthBarVisible();
    }

    private void UpdateHealthBarVisible() {
        if (healthSystem.IsFullHealth()) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
