using UnityEngine;

public interface IRequiresTargetSystem {
    public Transform GetTargetSearchOrigin();
    public float GetMaxRange();
    public TargetType GetTargetType();
    public float GetTargetScanInterval();
}
