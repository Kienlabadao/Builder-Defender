using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeInfoPlacementRulesUI : MonoBehaviour {
    [SerializeField] private Transform placementRuleTemplate;

    private Dictionary<BuildingManager.BuildingPlacementError, Transform> placementRuleTransformDictionary;

    private void Awake() {
        placementRuleTransformDictionary = new Dictionary<BuildingManager.BuildingPlacementError, Transform>();

        foreach (
            BuildingManager.BuildingPlacementError buildingPlacementError
            in Enum.GetValues(typeof(BuildingManager.BuildingPlacementError))
        ) {
            if (TryGetPlacementRuleText(buildingPlacementError, out string ruleText)) {
                Transform placementRuleTransform = Instantiate(placementRuleTemplate, transform);
                placementRuleTransform.GetComponent<BuildModeInfoPlacementRuleSingleUI>().SetRuleText(ruleText);

                placementRuleTransform.gameObject.SetActive(true);

                placementRuleTransformDictionary[buildingPlacementError] = placementRuleTransform;
            }
        }
    }

    private void Start() {
        BuildingGhost.Instance.OnInvalidPlacementPosition += BuildingGhost_OnInvalidPlacementPosition;
        BuildingGhost.Instance.OnValidPlacementPosition += BuildingGhost_OnValidPlacementPosition;

        placementRuleTemplate.gameObject.SetActive(false);

        Hide();
    }

    private void OnDestroy() {
        BuildingGhost.Instance.OnInvalidPlacementPosition -= BuildingGhost_OnInvalidPlacementPosition;
        BuildingGhost.Instance.OnValidPlacementPosition -= BuildingGhost_OnValidPlacementPosition;
    }

    private void BuildingGhost_OnValidPlacementPosition(object sender, System.EventArgs e) {
        foreach (Transform placementRuleTransform in placementRuleTransformDictionary.Values) {
            placementRuleTransform.GetComponent<BuildModeInfoPlacementRuleSingleUI>().SetValidVisual();
        }
    }

    private void BuildingGhost_OnInvalidPlacementPosition(
        object sender,
        BuildingGhost.OnInvalidPlacementPositionEventArgs e
    ) {
        foreach (
            KeyValuePair<BuildingManager.BuildingPlacementError, Transform> placementRule
            in placementRuleTransformDictionary
        ) {
            BuildModeInfoPlacementRuleSingleUI placementRuleSingleUI = placementRule.Value.GetComponent<BuildModeInfoPlacementRuleSingleUI>();

            if (e.buildingPlacementErrorList.Contains(placementRule.Key)) {
                placementRuleSingleUI.SetInvalidVisual();
            } else {
                placementRuleSingleUI.SetValidVisual();
            }
        }
    }

    private bool TryGetPlacementRuleText(
        BuildingManager.BuildingPlacementError buildingPlacementError,
        out string ruleText
    ) {
        ruleText = buildingPlacementError switch {
            BuildingManager.BuildingPlacementError.NoAciveBuilding => null,
            BuildingManager.BuildingPlacementError.CannotBePlaced => null,
            BuildingManager.BuildingPlacementError.AreaNotClear => "No overlapping objects",
            BuildingManager.BuildingPlacementError.TooCloseToSameBuilding => "Minimum distance from same-type buildings",
            BuildingManager.BuildingPlacementError.TooFarFromAnotherBuilding => "Maximum distance from another building",
            _ => throw new ArgumentOutOfRangeException(nameof(buildingPlacementError))
        };

        return ruleText != null;
    }

    public void Show() {
        gameObject.SetActive(true);

        foreach (Transform placementRuleTransform in placementRuleTransformDictionary.Values) {
            placementRuleTransform.GetComponent<BuildModeInfoPlacementRuleSingleUI>().SetInvalidVisual();
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
