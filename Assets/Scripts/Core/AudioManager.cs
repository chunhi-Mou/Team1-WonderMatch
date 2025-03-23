using UnityEngine;
using UnityEngine.Audio;
using System;
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
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
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
        LoadSettings();
    }
    #endregion
    
    private void InitSounds() {
        soundDict = new Dictionary<SoundEffect, AudioSource>();

        foreach (var sound in sounds) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
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
    public void SetMasterVolume(float volume) {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume) {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume) {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    private void LoadSettings() {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }
    public void SetUpSlider(Slider slider, string prefsKey) {
        float savedValue = PlayerPrefs.GetFloat(prefsKey, 1f);
        slider.SetValueWithoutNotify(savedValue);
    }

    public void SetUpToggle(Toggle toggle, string prefsKey, UnityAction<bool> toggleAction) {
        bool isOn = PlayerPrefs.GetInt(prefsKey, 1) == 1;
        toggle.isOn = isOn;
        toggle.onValueChanged.AddListener(toggleAction);
    }
    public void ToggleMusic(bool isOn) {
        float defaultVolume = 1f; 
        float volume = isOn ? defaultVolume : 0f; 
        SetMusicVolume(volume);
        PlayerPrefs.SetFloat(SavedData.MusicVolume, volume); 
        PlayerPrefs.Save();
    }
    public void ToggleSFX(bool isOn) {
        float defaultVolume = 1f; 
        float volume = isOn ? defaultVolume : 0f; 
        SetSFXVolume(volume);
        PlayerPrefs.SetFloat(SavedData.SFXVolume, volume); 
        PlayerPrefs.Save();
    }

    internal void SetUpToggle(object masterToggle, string v, Action<bool> value)
    {
        throw new NotImplementedException();
    }
}
