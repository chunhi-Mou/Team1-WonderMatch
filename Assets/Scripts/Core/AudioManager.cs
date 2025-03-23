using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour {
    [System.Serializable]
    public class Sound {
        public SoundEffect type;
        public AudioMixerGroup outputMixer;
        public AudioClip clip;
        public bool loop = false;
        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds;
    public AudioMixer audioMixer;
    private Dictionary<SoundEffect, AudioSource> soundDict;

    #region Singleton
    public static AudioManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
        InitSounds();
    }
    #endregion

    private void Start() {
        LoadSettings();
    }
    private void InitSounds() {
        soundDict = new Dictionary<SoundEffect, AudioSource>();

        foreach (var sound in sounds) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.outputMixer;

            sound.source = source;
            soundDict[sound.type] = source;
        }
    }
    public void Play(SoundEffect type) {
        if (soundDict.TryGetValue(type, out AudioSource source)) {
            source.Play();
        } else {
            Debug.LogWarning($"Sound {type} not found!");
        }
    }
    public void PlayOneShot(SoundEffect type) {
        if (soundDict.TryGetValue(type, out AudioSource source)) {
            source.PlayOneShot(source.clip);
        } else {
            Debug.LogWarning($"Sound {type} not found!");
        }
    }

    public void Pause(SoundEffect type) {
        if (soundDict.TryGetValue(type, out AudioSource source)) {
            source.Pause();
        }
    }
    public void PauseAll() {
        foreach (var source in soundDict.Values) {
            if (source.isPlaying) {
                source.Pause();
            }
        }
    }
    public void StopAll() {
        foreach (var source in soundDict.Values) {
            source.Stop();
        }
    }
    public void ResumeAll() {
        foreach (var source in soundDict.Values) {
            if (!source.isPlaying) {
                source.UnPause();
            }
        }
    }

    private void LoadSettings() {
        ToggleMusic(PlayerPrefs.GetInt(SavedData.MusicToggle, 1) == 1);
        ToggleSFX(PlayerPrefs.GetInt(SavedData.SFXToggle, 1) == 1);
    }
    public void SetUpToggle(Toggle toggle, string prefsKey, UnityAction<bool> toggleAction) {
        bool isOn = PlayerPrefs.GetInt(prefsKey, 1) == 1;
        toggle.isOn = isOn;
        toggle.onValueChanged.AddListener(toggleAction);
    }
    public void ToggleMusic(bool isOn) {
        audioMixer.SetFloat("MusicVolume", isOn ? 0f : -80f);
        PlayerPrefs.SetInt(SavedData.MusicToggle, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void ToggleSFX(bool isOn) {
        audioMixer.SetFloat("SFXVolume", isOn ? 0f : -80f);
        PlayerPrefs.SetInt(SavedData.SFXToggle, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
