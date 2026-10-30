using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private SpriteRenderer ghostSpriteRenderer;

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;

        Hide();
    }

    private void Update() {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void OnDestroy() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged -= BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        if (e.activeBuildingTypeSO) {
            Show(e.activeBuildingTypeSO.sprite);
        } else {
            Hide();
        }
    }

    private void Show(Sprite ghostSprite) {
        ghostSpriteRenderer.gameObject.SetActive(true);

        ghostSpriteRenderer.sprite = ghostSprite;
    }

    private void Hide() {
        ghostSpriteRenderer.gameObject.SetActive(false);
    }
}
