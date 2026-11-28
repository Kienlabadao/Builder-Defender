using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour {
    public static CoroutineRunner Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one CoroutineRunner instance!");
        }
        Instance = this;
    }

    public void RunCoroutine(IEnumerator coroutine) {
        StartCoroutine(coroutine);
    }
}