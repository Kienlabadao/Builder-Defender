using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image barImage;

    private void Awake() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnReset += HealthSystem_OnReset;
    }

    private void OnDestroy() {
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnHealed -= HealthSystem_OnHealed;
        healthSystem.OnReset -= HealthSystem_OnReset;
    }

    private void HealthSystem_OnReset(object sender, System.EventArgs e) {
        UpdateBar();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        UpdateBar();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        UpdateBar();
    }

    private void UpdateBar() {
        barImage.fillAmount = healthSystem.GetHealthAmountNormalized();

        UpdateHealthBarVisibility();
    }

    private void UpdateHealthBarVisibility() {
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
