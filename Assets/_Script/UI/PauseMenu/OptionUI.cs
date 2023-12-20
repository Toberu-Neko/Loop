using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour, IOptionData
{
    public event Action<bool> OnDeactivate;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider soundFXVolumeSlider;
    public void Activate()
    {
        DataPersistenceManager.Instance.LoadOptionData();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnDeactivate?.Invoke(false);
        DataPersistenceManager.Instance.SaveOptionData();
        // pauseUIMain.ActivateMenu();
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.instance.SetMasterVolume(Mathf.Log10(volume) * 20f);
    }

    public void SetSoundFXVolume(float volume)
    {
        AudioManager.instance.SetSoundFXVolume(Mathf.Log10(volume) * 20f);
    }

    public void SetBGMVolume(float volume)
    {
        AudioManager.instance.SetBGMVolume(Mathf.Log10(volume) * 20f);
    }

    public void LoadOptionData(OptionData data)
    {
        Debug.Log("Load");
        masterVolumeSlider.value = data.masterVolume;
        soundFXVolumeSlider.value = data.sfxVolume;
        bgmVolumeSlider.value = data.musicVolume;

        SetMasterVolume(data.masterVolume);
        SetSoundFXVolume(data.sfxVolume);
        SetBGMVolume(data.musicVolume);
    }

    public void SaveOptionData(OptionData data)
    {
        Debug.Log("Save");
        data.masterVolume = masterVolumeSlider.value;
        data.sfxVolume = soundFXVolumeSlider.value;
        data.musicVolume = bgmVolumeSlider.value;
    }
}
