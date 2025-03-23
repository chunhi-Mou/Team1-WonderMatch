using UnityEngine;
using UnityEngine.UI;


public class MusicHandler : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;
    void Start() 
    {
        AudioManager.instance.SetUpToggle(musicToggle, SavedData.MusicVolume, value => AudioManager.instance.ToggleMusic(value));
        AudioManager.instance.SetUpToggle(sfxToggle, "SFXToggle", value => AudioManager.instance.ToggleSFX(value));
    }

    public void ToggleMusic(bool isOn) {
        AudioManager.instance.ToggleMusic(isOn);
    }

    public void ToggleSFX(bool isOn) {
        AudioManager.instance.ToggleSFX(isOn);
    }
}
