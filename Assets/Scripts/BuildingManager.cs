using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs {
        public BuildingSO activeBuildingSO;
    }

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Building hqBuilding;
    [SerializeField] private float maxConstructionRadius = 25f;

    private BuildingSO activeBuildingSO;
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
                activeBuildingSO != null) {
                if (ResourceManager.Instance.CanAfford(activeBuildingSO.constructionResourceCostArray)) {
                    if (CanSpawnBuilding(activeBuildingSO, UtilsClass.GetMouseWorldPosition(), out string errorMessage)) {
                        Instantiate(activeBuildingSO.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);

                        ResourceManager.Instance.SpendResource(activeBuildingSO.constructionResourceCostArray);
                    } else {
                        TooltipUI.Instance.Show(errorMessage, true);
                    }
                } else {
                    TooltipUI.Instance.Show("Cannot afford!", activeBuildingSO.constructionResourceCostArray, true);
                }
            }
        }

        if (Keyboard.current.tKey.wasPressedThisFrame) {
            Vector3 enemySpawnPosition = UtilsClass.GetMouseWorldPosition() + UtilsClass.GetRandomDir() * 5f;
            Enemy.SpawnEnemy(enemySO, enemySpawnPosition);
        }
    }

    private void OnDestroy() {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        isInteractInputRecieved = true;
    }

    private bool CanSpawnBuilding(BuildingSO buildingSO, Vector3 position, out string errorMessage) {
        if (!buildingSO.canBePlaced) {
            errorMessage = $"Building {buildingSO.name} aren't allowed to be placed!";
            Debug.LogWarning(errorMessage);

            return false;
        }

        BoxCollider2D boxCollider2D = buildingSO.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0f);
        if (collider2DArray.Length != 0) {
            errorMessage = "Area is not clear!";

            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingSO.minConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray) {
            if (collider2D.TryGetComponent(out Building building) &&
                building.GetBuildingSO() == buildingSO) {
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

    public void SetActiveBuildingSO(BuildingSO buildingSO) {
        activeBuildingSO = buildingSO;

        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs {
            activeBuildingSO = activeBuildingSO
        });
    }

    public BuildingSO GetActiveBuildingSO() {
        return activeBuildingSO;
    }

    public Building GetHQBuilding() {
        return hqBuilding;
    }
}
