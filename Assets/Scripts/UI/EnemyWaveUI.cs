using TMPro;
using UnityEngine;

[RequireComponent(typeof(TargetingSystem))]
public class EnemyWaveUI : MonoBehaviour, IRequiresTargetSystem {
    private const float MAX_SEARCH_RADIUS = 9999f;

    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI waveMessageText;
    [SerializeField] private RectTransform enemyWaveSpawnPositionIndicatorRectTransform;
    [SerializeField] private RectTransform enemyClosestPositionIndicatorRectTransform;
    [SerializeField] private float enemyWaveSpawnPositionIndicatorPositionOffset = 300f;
    [SerializeField] private float enemyClosestPositionIndicatorPositionOffset = 250f;
    [SerializeField] private float indicatorHideOffset = 1.5f;

    private TargetingSystem targetingSystem;
    private bool isWaitingToSpawnNextWave = true;

    private void Awake() {
        targetingSystem = GetComponent<TargetingSystem>();
    }

    private void Start() {
        EnemyWaveManager.Instance.OnStateChanged += EnemyWaveManager_OnStateChanged;
        EnemyWaveManager.Instance.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;

        SetWaveNumberText(EnemyWaveManager.Instance.GetWaveNumber());
    }

    private void Update() {
        HandleEnemyClosestPositionIndicatorRectTransform();

        if (isWaitingToSpawnNextWave) {
            Vector3 spawnPosition = EnemyWaveManager.Instance.GetSpawnPosition();

            SetWaveMessageText(EnemyWaveManager.Instance.GetNextWaveSpawnTimer());
            HandleEnemyWaveSpawnPositionIndicatorRectTransform(spawnPosition);
        }
    }

    private void OnDestroy() {
        EnemyWaveManager.Instance.OnStateChanged -= EnemyWaveManager_OnStateChanged;
        EnemyWaveManager.Instance.OnWaveNumberChanged -= EnemyWaveManager_OnWaveNumberChanged;
    }

    private void EnemyWaveManager_OnStateChanged(object sender, System.EventArgs e) {
        isWaitingToSpawnNextWave = EnemyWaveManager.Instance.IsWaitingToSpawnNextWave();

        if (isWaitingToSpawnNextWave) {
            ShowWaitingToSpawnNextWaveVisual();
        } else {
            HideWaitingToSpawnNextWaveVisual();
        }
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e) {
        SetWaveNumberText(EnemyWaveManager.Instance.GetWaveNumber());
    }

    private void HandleEnemyWaveSpawnPositionIndicatorRectTransform(Vector3 spawnPosition) {
        Vector3 dirToNextSpawnPosition = (spawnPosition - Camera.main.transform.position).normalized;
        float distanceToNextSpawnPosition = Vector3.Distance(spawnPosition, Camera.main.transform.position);

        enemyWaveSpawnPositionIndicatorRectTransform.anchoredPosition = dirToNextSpawnPosition * enemyWaveSpawnPositionIndicatorPositionOffset;
        enemyWaveSpawnPositionIndicatorRectTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));
        enemyWaveSpawnPositionIndicatorRectTransform.gameObject.SetActive(distanceToNextSpawnPosition > Camera.main.orthographicSize * indicatorHideOffset);
    }

    private void HandleEnemyClosestPositionIndicatorRectTransform() {
        if (targetingSystem.GetCurrentTarget() is Enemy closestEnemy && closestEnemy != null) {
            Vector3 dirToClosestEnemy = (closestEnemy.transform.position - Camera.main.transform.position).normalized;
            float distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, Camera.main.transform.position);

            enemyClosestPositionIndicatorRectTransform.anchoredPosition = dirToClosestEnemy * enemyClosestPositionIndicatorPositionOffset;
            enemyClosestPositionIndicatorRectTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));
            enemyClosestPositionIndicatorRectTransform.gameObject.SetActive(distanceToClosestEnemy > Camera.main.orthographicSize * indicatorHideOffset);
        } else {
            enemyClosestPositionIndicatorRectTransform.gameObject.SetActive(false);
        }
    }

    private void SetWaveNumberText(int waveNumber) {
        waveNumberText.text = $"Wave {waveNumber}";
    }

    private void SetWaveMessageText(float nextWaveSpawnTimer) {
        waveMessageText.text = $"Next Wave in {Mathf.CeilToInt(nextWaveSpawnTimer)}s";
    }

    private void ShowWaitingToSpawnNextWaveVisual() {
        waveMessageText.gameObject.SetActive(true);
        enemyWaveSpawnPositionIndicatorRectTransform.gameObject.SetActive(true);
    }

    private void HideWaitingToSpawnNextWaveVisual() {
        waveMessageText.gameObject.SetActive(false);
        enemyWaveSpawnPositionIndicatorRectTransform.gameObject.SetActive(false);
    }

    public Transform GetTargetSearchOrigin() {
        return Camera.main.transform;
    }

    public float GetMaxRange() {
        return MAX_SEARCH_RADIUS;
    }

    public TargetType GetTargetType() {
        return TargetType.Enemy;
    }

    public float GetTargetScanInterval() {
        return 0f;
    }
}
