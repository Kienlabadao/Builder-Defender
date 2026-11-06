using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeSelectUI : MonoBehaviour {
    [SerializeField] private BuildingListSO buildingListSO;
    [SerializeField] private Transform buttonTemplate;

    private Transform arrowButton;
    private Dictionary<BuildingSO, Transform> buildingTypeTransformDictionary;

    private void Awake() {
        buildingTypeTransformDictionary = new Dictionary<BuildingSO, Transform>();

        arrowButton = InstantiateButtonTransform(null);

        foreach (BuildingSO buildingSO in buildingListSO.buildingSOArray) {
            if (buildingSO.canBePlaced) {
                Transform buttonTransform = InstantiateButtonTransform(buildingSO);

                buildingTypeTransformDictionary[buildingSO] = buttonTransform;
            }
        }
    }

    private void Start() {
        UpdateActiveBuildingTypeButton(BuildingManager.Instance.GetActiveBuildingSO());

        buttonTemplate.gameObject.SetActive(false);
    }

    private Transform InstantiateButtonTransform(BuildingSO buildingSO) {
        Transform buttonTransform = Instantiate(buttonTemplate, transform);
        BuildingTypeSelectSingleUI buildingTypeSelectSingleUI = buttonTransform.GetComponent<BuildingTypeSelectSingleUI>();

        buttonTransform.gameObject.SetActive(true);

        buildingTypeSelectSingleUI.SetButtonOnClickAction(() => {
            BuildingManager.Instance.SetActiveBuildingSO(buildingSO);

            UpdateActiveBuildingTypeButton(buildingSO);
        });

        if (buildingSO != null) {
            buildingTypeSelectSingleUI.SetBuildingSO(buildingSO);
            buildingTypeSelectSingleUI.SetOnMouseEnterAction(() => {
                TooltipUI.Instance.Show(buildingSO.buildingName, buildingSO.constructionResourceCostArray);
            });
            buildingTypeSelectSingleUI.SetOnMouseExitAction(() => {
                TooltipUI.Instance.Hide();
            });
        } else {
            buildingTypeSelectSingleUI.SetArrowButton();
        }

        return buttonTransform;
    }

    private void UpdateActiveBuildingTypeButton(BuildingSO activeBuildingSO) {
        foreach (BuildingSO buildingSO in buildingTypeTransformDictionary.Keys) {
            Transform buttonTransform = buildingTypeTransformDictionary[buildingSO];

            buttonTransform.GetComponent<BuildingTypeSelectSingleUI>().SetSelected(buildingSO == activeBuildingSO);
        }

        arrowButton.GetComponent<BuildingTypeSelectSingleUI>().SetSelected(activeBuildingSO == null);
    }
}
