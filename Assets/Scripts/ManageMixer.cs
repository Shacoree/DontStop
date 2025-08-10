using UnityEngine;
using UnityEngine.Audio;

public class ManageMixer : MonoBehaviour 
{
    [SerializeField] public AudioMixer mainAudioMixer;
    private string audioMixerMaster = "audioMixerMasterVolume";
    private string audioMixerMusic = "audioMixerMusicVolume";
    private string audioMixerSFX = "audioMixerSFXVolume";

    public static ManageMixer audioManager { get; private set; }

    private void Awake()
    {
        if (audioManager != null && audioManager != this)
        {
            Destroy(this);
        }
        else
        {
            audioManager = this;
        }
    }

    public void SetMasterVolume(float volume)
    {
        SetMixerVolume(audioMixerMaster, volume);
    }
    public void SetMusicVolume(float volume)
    {
        SetMixerVolume(audioMixerMusic, volume);
    }
    public void SetSFXVolume(float volume)
    {
        SetMixerVolume(audioMixerSFX, volume);
    }
    private void SetMixerVolume(string parameterName, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        mainAudioMixer.SetFloat(parameterName, dB);
    }
}
