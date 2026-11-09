using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Building : MonoBehaviour, IHasHealth, IDamageable, ITargetable {
    [SerializeField] private BuildingSO buildingSO;

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

    public BuildingSO GetBuildingSO() {
        return buildingSO;
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
}
