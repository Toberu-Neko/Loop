using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// This manager is responsible for changing the locale of the game.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private bool hasChangedLocale;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        hasChangedLocale = false;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeLocale(int index)
    {
        if (hasChangedLocale)
            return;
        StartCoroutine(SetLocale(index));
    }

    private IEnumerator SetLocale(int index)
    {
        hasChangedLocale = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        hasChangedLocale = false;
    }
}
