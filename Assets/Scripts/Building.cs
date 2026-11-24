using System;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Building : MonoBehaviour, IHasHealth, IDamageable, ITargetable, ISelectable {
    public event EventHandler OnBuildingUnhovered;
    public event EventHandler OnBuildingHovered;
    public event EventHandler OnBuildingSelected;
    public event EventHandler OnBuildingDeselected;
    public event EventHandler OnBuildingDestroyed;

    [SerializeField] private BuildingSO buildingSO;

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void OnDestroy() {
        healthSystem.OnDied -= HealthSystem_OnDied;
    }

    private void HealthSystem_OnDied(object sender, EventArgs e) {
        OnBuildingDestroyed?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    public BuildingSO GetBuildingSO() {
        return buildingSO;
    }

    public bool CanBeDemolished() {
        return GetBuildingSO().canBeDemolished;
    }

    public float GetHealthAmountMax() {
        return buildingSO.healthAmountMax;
    }

    public void Damage(float damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    public Vector3 GetTargetPosition() {
        return transform.position;
    }

    public void Hover() {
        OnBuildingHovered?.Invoke(this, EventArgs.Empty);
    }

    public void Unhover() {
        OnBuildingUnhovered?.Invoke(this, EventArgs.Empty);
    }

    public void Select() {
        OnBuildingSelected?.Invoke(this, EventArgs.Empty);
    }

    public void Deselect() {
        OnBuildingDeselected?.Invoke(this, EventArgs.Empty);
    }
}
