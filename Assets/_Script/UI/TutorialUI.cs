using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Video;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private LocalizeStringEvent titleStringEvent;
    [SerializeField] private LocalizeStringEvent descriptionStringEvent;
    public void OnClickContinue()
    {
        Deactivate();
    }

    public void Activate(VideoClip clip, LocalizedString titleString, LocalizedString descriptionString)
    {
        GameManager.Instance.PauseGame();
        videoPlayer.clip = clip;
        titleStringEvent.StringReference = titleString;
        descriptionStringEvent.StringReference = descriptionString;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
}
