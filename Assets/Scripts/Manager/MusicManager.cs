using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    public static MusicManager Instance { get; private set; }
    private static int volume = 5;
    private static float musicTime;

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private const int MAX_VOLUME = 10;

    public event EventHandler OnVolumeChanged;

    [SerializeField] private int defaultVolume = 5;

    private AudioSource musicAudioSource;

    private void Awake() {
        Instance = this;

        musicAudioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetInt(PLAYER_PREFS_MUSIC_VOLUME, defaultVolume);
        musicAudioSource.time = musicTime;
    }

    private void Start() {
        musicAudioSource.volume = GetVolumeNormalized();
    }

    private void Update() {
        musicTime = musicAudioSource.time;
    }

    public void ChangeVolume(int newVolume) {
        // 10 mod 10 = 0 so add 1 to get volume as max
        volume = newVolume % (MAX_VOLUME + 1);

        PlayerPrefs.SetInt(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();

        OnVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetVolume() {
        return volume;
    }

    public float GetVolumeNormalized() {
        return ((float)volume) / MAX_VOLUME;
    }
}
