using UnityEngine.SceneManagement;

public static class SceneLoader {
    public enum Scene {
        MainMenuScene,
        GameScene,
    }

    public static void Load(Scene targetScene) {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
