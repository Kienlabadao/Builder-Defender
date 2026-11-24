using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI wavesSurvivedText;

    private void Awake() {
        restartButton.onClick.AddListener(() => {
            SceneLoader.Load(SceneLoader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener(() => {
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void OnDestroy() {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        int wavesSurvived = EnemyWaveManager.Instance.GetWavesSurvivedNumber();
        string waveLabel = wavesSurvived == 1 ? "Wave" : "Waves";

        wavesSurvivedText.text = $"You Survived {wavesSurvived} {waveLabel}!";

        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
