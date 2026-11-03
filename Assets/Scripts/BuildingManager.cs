using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs {
        public BuildingTypeSO activeBuildingTypeSO;
    }

    [SerializeField] private EnemyTypeSO enemyTypeSO;
    [SerializeField] private Building hqBuilding;
    [SerializeField] private float maxConstructionRadius = 25f;

    private BuildingTypeSO activeBuildingTypeSO;
    private bool isInteractInputRecieved = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one BuildingManager instance!");
        }
        Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update() {
        if (isInteractInputRecieved) {
            isInteractInputRecieved = false;

            if (!EventSystem.current.IsPointerOverGameObject() &&
                activeBuildingTypeSO != null) {
                if (ResourceManager.Instance.CanAfford(activeBuildingTypeSO.constructionResourceCostArray)) {
                    if (CanSpawnBuilding(activeBuildingTypeSO, UtilsClass.GetMouseWorldPosition(), out string errorMessage)) {
                        Instantiate(activeBuildingTypeSO.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

                        ResourceManager.Instance.SpendResource(activeBuildingTypeSO.constructionResourceCostArray);
                    } else {
                        TooltipUI.Instance.Show(errorMessage, true);
                    }
                } else {
                    TooltipUI.Instance.Show("Cannot afford!", activeBuildingTypeSO.constructionResourceCostArray, true);
                }
            }
        }

        if (Keyboard.current.tKey.wasPressedThisFrame) {
            Vector3 enemySpawnPosition = UtilsClass.GetMouseWorldPosition() + UtilsClass.GetRandomDir() * 5f;
            Enemy.SpawnEnemy(enemyTypeSO, enemySpawnPosition);
        }
    }

    private void OnDestroy() {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        isInteractInputRecieved = true;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingTypeSO, Vector3 position, out string errorMessage) {
        if (!buildingTypeSO.canBePlaced) {
            errorMessage = $"Building {buildingTypeSO.name} aren't allowed to be placed!";
            Debug.LogWarning(errorMessage);

            return false;
        }

        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0f);
        if (collider2DArray.Length != 0) {
            errorMessage = "Area is not clear!";

            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingTypeSO.minConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out Building building) &&
                building.GetBuildingTypeSO() == buildingTypeSO) {
                errorMessage = "Too close to another building of the same type!";
                return false;
            }
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out Building _)) {
                errorMessage = null;

                return true;
            }
        }

        errorMessage = "Too far from any other building!";
        return false;
    }

    public void SetActiveBuildingTypeSO(BuildingTypeSO buildingTypeSO) {
        activeBuildingTypeSO = buildingTypeSO;

        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs {
            activeBuildingTypeSO = activeBuildingTypeSO
        });
    }

    public BuildingTypeSO GetActiveBuildingTypeSO() {
        return activeBuildingTypeSO;
    }

    public Building GetHQBuilding() {
        return hqBuilding;
    }
}
