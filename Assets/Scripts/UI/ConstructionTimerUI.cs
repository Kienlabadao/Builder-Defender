using UnityEngine;

public class ConstructionTimerUI : MonoBehaviour {
    [SerializeField] private BuildingConstruction buildingConstruction;
    [SerializeField] private ProgressUI progressUI;

    private void Update() {
        progressUI.SetProgressAmount(1 - buildingConstruction.GetContructionTimerNormalized());
    }
}
