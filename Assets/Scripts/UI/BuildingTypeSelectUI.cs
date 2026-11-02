using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeSelectUI : MonoBehaviour {
    [SerializeField] private BuildingTypeListSO buildingTypeListSO;
    [SerializeField] private Transform buttonTemplate;

    private Transform arrowButton;
    private Dictionary<BuildingTypeSO, Transform> buildingTypeTransformDictionary;

    private void Awake() {
        buildingTypeTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        arrowButton = InstantiateButtonTransform(null);

        foreach (BuildingTypeSO buildingTypeSO in buildingTypeListSO.buildingTypeSOList) {
            if (buildingTypeSO.canBePlaced) {
                Transform buttonTransform = InstantiateButtonTransform(buildingTypeSO);

                buildingTypeTransformDictionary[buildingTypeSO] = buttonTransform;
            }
        }
    }

    private void Start() {
        UpdateActiveBuildingTypeButton(BuildingManager.Instance.GetActiveBuildingTypeSO());

        buttonTemplate.gameObject.SetActive(false);
    }

    private Transform InstantiateButtonTransform(BuildingTypeSO buildingTypeSO) {
        Transform buttonTransform = Instantiate(buttonTemplate, transform);
        BuildingTypeSelectSingleUI buildingTypeSelectSingleUI = buttonTransform.GetComponent<BuildingTypeSelectSingleUI>();

        buttonTransform.gameObject.SetActive(true);

        buildingTypeSelectSingleUI.SetButtonOnClickAction(() => {
            BuildingManager.Instance.SetActiveBuildingTypeSO(buildingTypeSO);

            UpdateActiveBuildingTypeButton(buildingTypeSO);
        });

        if (buildingTypeSO != null) {
            buildingTypeSelectSingleUI.SetBuildingTypeSO(buildingTypeSO);
            buildingTypeSelectSingleUI.SetOnMouseEnterAction(() => {
                TooltipUI.Instance.Show(buildingTypeSO.buildingName, buildingTypeSO.constructionResourceCostArray);
            });
            buildingTypeSelectSingleUI.SetOnMouseExitAction(() => {
                TooltipUI.Instance.Hide();
            });
        } else {
            buildingTypeSelectSingleUI.SetArrowButton();
        }

        return buttonTransform;
    }

    private void UpdateActiveBuildingTypeButton(BuildingTypeSO activeBuildingTypeSO) {
        foreach (BuildingTypeSO buildingTypeSO in buildingTypeTransformDictionary.Keys) {
            Transform buttonTransform = buildingTypeTransformDictionary[buildingTypeSO];

            buttonTransform.GetComponent<BuildingTypeSelectSingleUI>().SetSelected(buildingTypeSO == activeBuildingTypeSO);
        }

        arrowButton.GetComponent<BuildingTypeSelectSingleUI>().SetSelected(activeBuildingTypeSO == null);
    }
}
