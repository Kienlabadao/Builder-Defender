using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    [SerializeField] private GameObject hasHealthGameObject;

    public event EventHandler OnDamaged;
    public event EventHandler OnDied;

    private IHasHealth hasHealth;
    private float healthAmountMax;
    private float healthAmount;

    private void Awake() {
        hasHealth = hasHealthGameObject.GetComponent<IHasHealth>();
        if (hasHealth == null) {
            Debug.LogError($"Game Object {hasHealthGameObject} does not have a component that implements IHasHealth!");
        }
    }

    private void Start() {
        healthAmountMax = hasHealth.GetHealthAmountMax();
        healthAmount = healthAmountMax;
    }

    public void Damage(float damageAmount) {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0f, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (IsDead()) {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsDead() {
        return healthAmount == 0f;
    }

    public bool IsFullHealth() {
        return healthAmount == healthAmountMax;
    }

    public float GetHealthAmount() {
        return healthAmount;
    }

    public float GetHealthAmountNormalized() {
        return (float)healthAmount / healthAmountMax;
    }
}
