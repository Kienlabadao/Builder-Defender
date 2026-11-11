public class SpawnDatabase {
    private static SpawnDatabaseSO spawnDatabaseSO;

    public static void Initialize(SpawnDatabaseSO spawnDatabaseSO) {
        SpawnDatabase.spawnDatabaseSO = spawnDatabaseSO;
    }

    public static SpawnDatabaseSO GetSpawnDatabaseSO() {
        return spawnDatabaseSO;
    }
}
