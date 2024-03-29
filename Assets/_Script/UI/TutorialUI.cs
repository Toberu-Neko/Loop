using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    public void OnClickContinue()
    {
        Deactivate();
    }

    public void Activate(VideoClip clip)
    {
        GameManager.Instance.PauseGame();
        videoPlayer.clip = clip;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
}
