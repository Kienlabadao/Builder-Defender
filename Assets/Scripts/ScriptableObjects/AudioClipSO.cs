using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipSO", menuName = "Scriptable Objects/AudioClipSO")]
public class AudioClipSO : ScriptableObject {
    public AudioClip[] audioClipArray;
    public bool loop = false;
}
