using UnityEngine;
using UnityEngine.UI;


public class MusicHandler : MonoBehaviour
{
    public Toggle musicToggle;
    public GearSpin musicSpin;
    public Image musicImage;

    public Toggle sfxToggle;
    public GearSpin sfxSpin;
    public Image sfxImage;
    void Start() 
    {
        sfxImage.color = Color.white;
        musicImage.color = Color.white;
        VisualHandler(musicImage, musicSpin, PlayerPrefs.GetInt(SavedData.MusicToggle, 1) == 1);
        VisualHandler(sfxImage, sfxSpin, PlayerPrefs.GetInt(SavedData.SFXToggle, 1) == 1);
        AudioManager.instance.SetUpToggle(musicToggle, SavedData.MusicToggle, value => AudioManager.instance.ToggleMusic(value));
        AudioManager.instance.SetUpToggle(sfxToggle, SavedData.SFXToggle, value => AudioManager.instance.ToggleSFX(value));
    }

    public void ToggleMusic(bool isOn) {
        VisualHandler(musicImage, musicSpin, isOn);
        AudioManager.instance.ToggleMusic(isOn);
    }

    public void ToggleSFX(bool isOn) {
        VisualHandler(sfxImage, sfxSpin, isOn);
        AudioManager.instance.ToggleSFX(isOn);
    }
    private void VisualHandler(Image image, GearSpin _spin, bool _isOn) {
        image.color = _isOn ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        _spin.enabled = _isOn;
    }
}
