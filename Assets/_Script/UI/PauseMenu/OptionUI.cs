using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Localization.Settings;

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
        if(hasChangedLocale || !gameObject.activeInHierarchy)
            return;
        languageIndex = index;
        StartCoroutine(SetLocale(index));
    }

    private IEnumerator SetLocale(int index)
    {
        hasChangedLocale = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        hasChangedLocale = false;
    }

    public void LoadOptionData(OptionData data)
    {
        Debug.Log("Load");
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
        Debug.Log("Save");
        data.languageIndex = languageIndex;
        data.masterVolume = masterVolumeSlider.value;
        data.sfxVolume = soundFXVolumeSlider.value;
        data.musicVolume = bgmVolumeSlider.value;
    }
}
