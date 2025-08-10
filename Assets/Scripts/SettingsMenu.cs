using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor.Audio;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public UnityEngine.UI.Slider masterVolumeSlider, musicVolumeSlider, SFXVolumeSlider;
    [SerializeField] public UnityEngine.UI.Slider sensitivitySlider;

    public static string prefsMasterVolume = "masterVolume";
    public static string prefsMusicVolume = "musicVolume";
    public static string prefsSFXVolume = "sfxVolume";
    public static string prefsSensitivity = "playerSensitivity";

    private bool isLoading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerPrefs.GetFloat(prefsSensitivity));
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void ChangeMasterVolume()
    {
        if (isLoading) return; // ignore events during load
        ManageMixer.audioManager.SetMasterVolume(masterVolumeSlider.value);
        Save();
    }
    public void ChangeMusicVolume()
    {
        if (isLoading) return; // ignore events during load
        ManageMixer.audioManager.SetMusicVolume(musicVolumeSlider.value);
        Save();
    }
    public void ChangeSFXVolume()
    {
        if (isLoading) return; // ignore events during load
        ManageMixer.audioManager.SetSFXVolume(SFXVolumeSlider.value);
        Save();
    }
    public void ChangeSensitivity()
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


        ManageMixer.audioManager.SetMasterVolume(masterVolumeSlider.value);
        ManageMixer.audioManager.SetMusicVolume(musicVolumeSlider.value);
        ManageMixer.audioManager.SetSFXVolume(SFXVolumeSlider.value);

        isLoading = false;
    }
    private void Save()
    {
        PlayerPrefs.SetFloat(prefsMasterVolume, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(prefsMusicVolume, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(prefsSFXVolume, SFXVolumeSlider.value);
        PlayerPrefs.SetFloat(prefsSensitivity, sensitivitySlider.value);
        PlayerPrefs.Save();
    }
}
