using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [SerializeField] private SpawnDatabaseSO spawnDatabaseSO;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one GameManager instance!");
        }
        Instance = this;

        SpawnDatabase.Initialize(spawnDatabaseSO);
    }
}
