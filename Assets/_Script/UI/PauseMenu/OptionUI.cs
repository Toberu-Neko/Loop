using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour, IOptionData
{
    public event Action<bool> OnDeactivate;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider soundFXVolumeSlider;

    private bool hasChangedLocale;
    private int languageIndex;

    public void Activate()
    {
        DataPersistenceManager.Instance.LoadOptionData();
        gameObject.SetActive(true);
        hasChangedLocale = false;
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

    public void ChangeLocaleButton(int index)
    {
        languageIndex = index;
        LocalizationManager.Instance.ChangeLocale(index);
    }


    public void LoadOptionData(OptionData data)
    {
        Debug.Log("Load Option");
        languageIndex = data.languageIndex;
        masterVolumeSlider.value = data.masterVolume;
        soundFXVolumeSlider.value = data.sfxVolume;
        bgmVolumeSlider.value = data.musicVolume;

        SetMasterVolume(data.masterVolume);
        SetSoundFXVolume(data.sfxVolume);
        SetBGMVolume(data.musicVolume);
        ChangeLocaleButton(data.languageIndex);
    }

    public void SaveOptionData(OptionData data)
    {
        Debug.Log("Save Option");
        data.languageIndex = languageIndex;
        data.masterVolume = masterVolumeSlider.value;
        data.sfxVolume = soundFXVolumeSlider.value;
        data.musicVolume = bgmVolumeSlider.value;
    }
}
