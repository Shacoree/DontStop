using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public UnityEngine.UI.Slider masterVolumeSlider, musicVolumeSlider, SFXVolumeSlider;
    [SerializeField] public UnityEngine.UI.Slider sensitivitySlider;
    [SerializeField] public UnityEngine.UI.Slider FOVSlider;


    public static string prefsMasterVolume = "masterVolume";
    public static string prefsMusicVolume = "musicVolume";
    public static string prefsSFXVolume = "sfxVolume";
    public static string prefsSensitivity = "playerSensitivity";
    public static string prefsFOV = "playerFOV";

    private bool isLoading = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey(prefsMasterVolume))
        {
            PlayerPrefs.SetFloat(prefsMasterVolume, 1);
        }
        if (!PlayerPrefs.HasKey(prefsMusicVolume))
        {
            PlayerPrefs.SetFloat(prefsMusicVolume, 1);
        }
        if (!PlayerPrefs.HasKey(prefsSFXVolume))
        {
            PlayerPrefs.SetFloat(prefsSFXVolume, 1);
        }
        if (!PlayerPrefs.HasKey(prefsSensitivity))
        {
            PlayerPrefs.SetFloat(prefsSensitivity, 1);
        }
        if (!PlayerPrefs.HasKey(prefsFOV))
        {
            PlayerPrefs.SetFloat(prefsFOV, 90);
        }
        Load();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void ChangeMasterVolume()
    {
        if (isLoading) return; // ignore events during load
        if (ManageMixer.audioManager != null)
            ManageMixer.audioManager.SetMasterVolume(masterVolumeSlider.value);
        Save();
    }
    public void ChangeMusicVolume()
    {
        if (isLoading) return; // ignore events during load
        if (ManageMixer.audioManager != null)
            ManageMixer.audioManager.SetMusicVolume(musicVolumeSlider.value);
        Save();
    }
    public void ChangeSFXVolume()
    {
        if (isLoading) return; // ignore events during load
        if (ManageMixer.audioManager != null)
            ManageMixer.audioManager.SetSFXVolume(SFXVolumeSlider.value);
        Save();
    }
    public void ChangeSensitivity()
    {
        if (isLoading) return; // ignore events during load
        Save();
    }
    public void ChangeFOV()
    {
        if (isLoading) return; // ignore events during load
        Save();
    }
    private void Load()
    {
        isLoading = true;

        masterVolumeSlider.value = PlayerPrefs.GetFloat(prefsMasterVolume);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(prefsMusicVolume);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat(prefsSFXVolume);
        sensitivitySlider.value = PlayerPrefs.GetFloat(prefsSensitivity);
        FOVSlider.value = PlayerPrefs.GetFloat(prefsFOV);

        if (ManageMixer.audioManager != null)
        {
            ManageMixer.audioManager.SetMasterVolume(masterVolumeSlider.value);
            ManageMixer.audioManager.SetMusicVolume(musicVolumeSlider.value);
            ManageMixer.audioManager.SetSFXVolume(SFXVolumeSlider.value);
        }

        isLoading = false;
    }
    private void Save()
    {
        PlayerPrefs.SetFloat(prefsMasterVolume, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(prefsMusicVolume, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(prefsSFXVolume, SFXVolumeSlider.value);
        PlayerPrefs.SetFloat(prefsSensitivity, sensitivitySlider.value);
        PlayerPrefs.SetFloat(prefsFOV, FOVSlider.value);
        PlayerPrefs.Save();
    }
}
