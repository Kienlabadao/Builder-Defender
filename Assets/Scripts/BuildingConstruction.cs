using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BuildingConstruction : MonoBehaviour {
    private BoxCollider2D boxCollider2D;
    private BuildingSO buildingSO;
    private float constructionTimerMax;
    private float constructionTimer;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        constructionTimer -= Time.deltaTime;

        if (constructionTimer <= 0f) {
            Instantiate(buildingSO.prefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void SetBuildingSO(BuildingSO buildingSO) {
        this.buildingSO = buildingSO;

        this.constructionTimerMax = buildingSO.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        BoxCollider2D buildingBoxCollider2D = buildingSO.prefab.GetComponent<BoxCollider2D>();

        boxCollider2D.offset = buildingBoxCollider2D.offset;
        boxCollider2D.size = buildingBoxCollider2D.size;
    }

    public float GetContructionTimerNormalized() {
        return constructionTimer / constructionTimerMax;
    }

    public BuildingSO GetBuildingSO() {
        return buildingSO;
    }

    public static BuildingConstruction SpawnBuildingConstruction(Vector3 position, BuildingSO buildingSO) {
        Transform buildingConstructionTransform = Instantiate(
            SpawnDatabase.GetSpawnDatabaseSO().buildingConstructionPrefab,
            position,
            Quaternion.identity
        );
        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();

        buildingConstruction.SetBuildingSO(buildingSO);

        return buildingConstruction;
    }
}
