[System.Serializable]
public class WaveSpawningData {
    public EnemySO enemySO;
    public int spawnAmount;

    public WaveSpawningData(EnemySO enemySO, int spawnAmount) {
        this.enemySO = enemySO;
        this.spawnAmount = spawnAmount;
    }

    public WaveSpawningData(WaveSpawningData waveSpawningData) {
        this.enemySO = waveSpawningData.enemySO;
        this.spawnAmount = waveSpawningData.spawnAmount;
    }
}
