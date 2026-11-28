using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    private const int AUDIO_SOURCE_POOL_INITIAL_SIZE = 20;
    private const int AUDIO_SOURCE_POOL_MAX_SIZE = 100;
    private const int MAX_VOLUME = 10;

    public enum Sound {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }

    public event EventHandler OnVolumeChanged;

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int defaultVolume = 5;

    private int volume;
    private PrefabPool<AudioSource> audioSourcePool;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one SoundManager instance!");
        }
        Instance = this;

        audioSourcePool = new PrefabPool<AudioSource>(
            audioSourcePrefab,
            transform,
            AUDIO_SOURCE_POOL_INITIAL_SIZE,
            AUDIO_SOURCE_POOL_MAX_SIZE
        );
        volume = PlayerPrefs.GetInt(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, defaultVolume);
    }

    private AudioSource PlaySound(AudioClipSO audioClipSO, Vector3 position, float volume = 1f) {
        AudioSource audioSource = audioSourcePool.Get();

        AudioClip[] audioClipArray = audioClipSO.audioClipArray;
        AudioClip audioClip = audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];

        bool loop = audioClipSO.loop;

        audioSource.transform.position = position;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;

        audioSource.Play();

        if (!loop) {
            audioSourcePool.Release(audioSource, audioClip.length / Mathf.Abs(audioSource.pitch));
        }

        return audioSource;
    }

    public void PlaySound(Sound sound) {
        AudioClipSO audioClipSO = sound switch {
            Sound.BuildingPlaced => audioClipRefsSO.buildingPlaced,
            Sound.BuildingDamaged => audioClipRefsSO.buildingDamaged,
            Sound.BuildingDestroyed => audioClipRefsSO.buildingDestroyed,
            Sound.EnemyDie => audioClipRefsSO.enemyDie,
            Sound.EnemyHit => audioClipRefsSO.enemyHit,
            Sound.GameOver => audioClipRefsSO.gameOver,
            _ => throw new ArgumentOutOfRangeException(nameof(sound)),
        };

        PlaySound(audioClipSO, Camera.main.transform.position, GetVolumeNormalized());
    }

    public void ChangeVolume(int newVolume) {
        // 10 mod 10 = 0 so add 1 to get volume as max
        volume = newVolume % (MAX_VOLUME + 1);

        PlayerPrefs.SetInt(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
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
