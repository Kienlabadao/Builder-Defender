using UnityEngine;

//[CreateAssetMenu(fileName = "AudioClipRefsSO", menuName = "Scriptable Objects/AudioClipRefsSO")]
public class AudioClipRefsSO : ScriptableObject {
    public AudioClipSO buildingPlaced;
    public AudioClipSO buildingDamaged;
    public AudioClipSO buildingDestroyed;
    public AudioClipSO enemyDie;
    public AudioClipSO enemyHit;
    public AudioClipSO gameOver;
}
