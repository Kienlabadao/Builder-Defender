using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeSelectUI : MonoBehaviour {
    [SerializeField] private BuildingTypeListSO buildingTypeListSO;
    [SerializeField] private Transform buttonTemplate;

    private Transform arrowButton;
    private Dictionary<BuildingTypeSO, Transform> buildingTypeTransformDictionary;

    private void Awake() {
        buildingTypeTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        buttonTemplate.gameObject.SetActive(false);

        arrowButton = InstantiateButtonTransform(null);

        foreach (BuildingTypeSO buildingTypeSO in buildingTypeListSO.buildingTypeSOList) {
            Transform buttonTransform = InstantiateButtonTransform(buildingTypeSO);

            buildingTypeTransformDictionary[buildingTypeSO] = buttonTransform;
        }
    }

    private void Start() {
        UpdateActiveBuildingTypeButton(BuildingManager.Instance.GetActiveBuildingTypeSO());
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
