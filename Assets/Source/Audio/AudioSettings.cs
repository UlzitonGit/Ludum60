using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings: MonoBehaviour
{
 
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume")) LoadMusicVolume();
        else SetMusicVolume();

        if (PlayerPrefs.HasKey("SFXVolume")) LoadSFXVolume();
        else SetSFXVolume();
    }
    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _audioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        _audioMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    private void LoadMusicVolume()
    {        
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
    private void LoadSFXVolume()
    {       
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume();
    }
}