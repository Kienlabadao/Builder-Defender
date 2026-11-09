using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {
    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveNumberChanged;
    public event EventHandler OnStateChanged;

    public enum State {
        WaitingToSpawnNextWave,
        SpawningWave,
        GameOver,
    }

    [SerializeField] private WaveSpawningListSO waveSpawningListSO;
    [SerializeField] private Transform[] spawnPositionTransformArray;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    [SerializeField] private bool isInfiniteWave;
    [SerializeField] private Dictionary<EnemySO, int> extraWaveSpawnExtraAmountDictionary;
    [SerializeField] private float nextWaveSpawnTimerMax = 10f;
    [SerializeField] private float nextEnemySpawnTimerMaxOffset = .2f;
    [SerializeField] private float enemySpawnPositionMaxOffset = 10f;

    private State state = State.WaitingToSpawnNextWave;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private WaveSpawningSO[] waveSpawningSOArray;
    private WaveSpawningData[] lastDefinedWaveSpawningDataArray;
    private List<WaveSpawningData> currentWaveSpawningDataList;
    private Transform spawnPositionTransform;
    private int waveNumber = 0;
    private int extraWaveNumber = 0;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Debug.LogError("There is more than one EnemyWaveManager instance!");
        }
        Instance = this;

        nextWaveSpawnTimer = nextWaveSpawnTimerMax;
        currentWaveSpawningDataList = new List<WaveSpawningData>();
        waveSpawningSOArray = waveSpawningListSO.waveSpawningSOArray;
        lastDefinedWaveSpawningDataArray = waveSpawningSOArray[waveSpawningSOArray.Length - 1].waveSpawningDataArray;
        SetSpawningPositionTransform();
    }

    private void Update() {
        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;

                if (nextWaveSpawnTimer <= 0f) {
                    ChangeState(State.SpawningWave);
                }

                break;
            case State.SpawningWave:
                if (currentWaveSpawningDataList.Count > 0) {
                    nextEnemySpawnTimer -= Time.deltaTime;

                    if (nextEnemySpawnTimer <= 0f) {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, nextEnemySpawnTimerMaxOffset);

                        Vector3 spawnPosition = spawnPositionTransform.position + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, enemySpawnPositionMaxOffset);
                        int waveSpawningDataIndex = UnityEngine.Random.Range(0, currentWaveSpawningDataList.Count - 1);
                        WaveSpawningData waveSpawningData = currentWaveSpawningDataList[waveSpawningDataIndex];

                        Enemy.SpawnEnemy(waveSpawningData.enemySO, spawnPosition);
                        waveSpawningData.spawnAmount--;

                        if (waveSpawningData.spawnAmount <= 0) {
                            currentWaveSpawningDataList.Remove(waveSpawningData);
                        }
                    }
                } else {
                    ChangeState(State.WaitingToSpawnNextWave);
                }

                break;
            case State.GameOver:
                break;
            default:
                throw new NotImplementedException($"State {state} not implemented!");
        }
    }

    private void SpawnWave() {
        waveNumber++;

        if (waveNumber <= waveSpawningSOArray.Length) {
            foreach (WaveSpawningData waveSpawningData in waveSpawningSOArray[waveNumber - 1].waveSpawningDataArray) {
                currentWaveSpawningDataList.Add(new WaveSpawningData(waveSpawningData));
            }
        } else if (isInfiniteWave) {
            extraWaveNumber++;

            foreach (WaveSpawningData waveSpawningData in lastDefinedWaveSpawningDataArray) {
                if (extraWaveSpawnExtraAmountDictionary.TryGetValue(waveSpawningData.enemySO, out int spawnAmount)) {
                    WaveSpawningData newWaveSpawningData = new WaveSpawningData(waveSpawningData);
                    newWaveSpawningData.spawnAmount += spawnAmount * extraWaveNumber;

                    currentWaveSpawningDataList.Add(newWaveSpawningData);
                } else {
                    currentWaveSpawningDataList.Add(new WaveSpawningData(waveSpawningData));
                }
            }
        } else {
            ChangeState(State.GameOver);
        }

        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetSpawningPositionTransform() {
        spawnPositionTransform = spawnPositionTransformArray[UnityEngine.Random.Range(0, spawnPositionTransformArray.Length)];
        nextWaveSpawnPositionTransform.position = spawnPositionTransform.position;
    }

    private void ChangeState(State state) {
        this.state = state;

        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer = nextWaveSpawnTimerMax;

                SetSpawningPositionTransform();

                break;
            case State.SpawningWave:
                SpawnWave();

                break;
            case State.GameOver:
                break;
            default:
                throw new NotImplementedException($"State {state} not implemented!");
        }

        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsWaitingToSpawnNextWave() {
        return state == State.WaitingToSpawnNextWave;
    }

    public float GetNextWaveSpawnTimer() {
        return nextWaveSpawnTimer;
    }

    public int GetWaveNumber() {
        return waveNumber;
    }

    public Vector3 GetSpawnPosition() {
        return spawnPositionTransform.position;
    }
}
