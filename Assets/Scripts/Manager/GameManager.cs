using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    private enum State {
        GamePlaying,
        GameOver,
    }

    public event EventHandler OnStateChanged;

    [SerializeField] private SpawnDatabaseSO spawnDatabaseSO;
    private State state = State.GamePlaying;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one GameManager instance!");
        }
        Instance = this;

        SpawnDatabase.Initialize(spawnDatabaseSO);
    }

    private void Start() {
        BuildingManager.Instance.OnHQBuildingDestroyed += BuildingManager_OnHQBuildingDestroyed;
    }

    private void OnDestroy() {
        BuildingManager.Instance.OnHQBuildingDestroyed += BuildingManager_OnHQBuildingDestroyed;
    }

    private void BuildingManager_OnHQBuildingDestroyed(object sender, System.EventArgs e) {
        ChangeState(State.GameOver);
    }

    private void ChangeState(State state) {
        this.state = state;

        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }
}
