using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Building : MonoBehaviour, IHasHealth, IDamageable {
    [SerializeField] private BuildingTypeSO buildingTypeSO;

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void OnDestroy() {
        healthSystem.OnDied -= HealthSystem_OnDied;
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Destroy(gameObject);
    }

    public BuildingTypeSO GetBuildingTypeSO() {
        return buildingTypeSO;
    }

    public float GetHealthAmountMax() {
        return buildingTypeSO.healthAmountMax;
    }

    public void Damage(float damageAmount) {
        healthSystem.Damage(damageAmount);
    }
}
