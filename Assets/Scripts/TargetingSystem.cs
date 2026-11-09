using UnityEngine;

public class TargetingSystem : MonoBehaviour {
    [SerializeField] private GameObject requiresTargetSystemGameObject;

    private IRequiresTargetSystem requiresTargetSystem;
    private ITargetable currentTarget;
    private TargetType targetType;
    private Transform targetSearchOrigin;
    private float maxRange;
    private float lookForTargetTimerMax;
    private float lookForTargetTimer;

    private void Awake() {
        requiresTargetSystem = requiresTargetSystemGameObject.GetComponent<IRequiresTargetSystem>();
        if (requiresTargetSystem == null) {
            Debug.LogError($"Game Object {requiresTargetSystem} does not have a component that implements IRequiresTargetSystem!");
        }
    }

    private void Start() {
        targetSearchOrigin = requiresTargetSystem.GetTargetSearchOrigin();
        maxRange = requiresTargetSystem.GetMaxRange();
        targetType = requiresTargetSystem.GetTargetType();
        lookForTargetTimerMax = requiresTargetSystem.GetTargetScanInterval();
        lookForTargetTimer = lookForTargetTimerMax;
    }

    private void Update() {
        lookForTargetTimer -= Time.deltaTime;

        if (lookForTargetTimer <= 0f) {
            lookForTargetTimer = lookForTargetTimerMax;

            LookForTarget();
        }
    }

    private void LookForTarget() {
        Vector3 searchPosition = targetSearchOrigin != null
            ? targetSearchOrigin.position
            : transform.position;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(searchPosition, maxRange);

        float closestDistance = float.MaxValue;
        ITargetable target = null;

        foreach (Collider2D collider2D in collider2DArray) {
            if (!IsValidTarget(collider2D, out ITargetable foundTarget)) {
                continue;
            }

            float distance = Vector3.Distance(
                searchPosition,
                foundTarget.GetTargetPosition()
            );

            if (distance < closestDistance) {
                target = foundTarget;
                closestDistance = distance;
            }
        }

        currentTarget = target;
    }

    private bool IsValidTarget(Collider2D collider2D, out ITargetable target) {
        target = null;

        if (targetType.HasFlag(TargetType.Building) && collider2D.TryGetComponent(out Building building)) {
            target = building;

            return true;
        }

        if (targetType.HasFlag(TargetType.Enemy) && collider2D.TryGetComponent(out Enemy enemy)) {
            target = enemy;

            return true;
        }

        return false;
    }

    public ITargetable GetCurrentTarget() {
        Debug.Log(targetSearchOrigin.position);
        Debug.Log(currentTarget);
        return currentTarget;
    }
}
