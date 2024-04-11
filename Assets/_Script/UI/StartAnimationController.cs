using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Video;

public class StartAnimationController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private VideoClip zhVid;
    [SerializeField] private VideoClip jpVid;
    [SerializeField] private VideoClip enVid;

    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField] private GameObject loadingObj;

    private bool skipped;

    private void Awake()
    {
        skipped = false;

        if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            Debug.Log("ZH");
            videoPlayer.clip = zhVid;
        }
        else if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            Debug.Log("JP");
            videoPlayer.clip = jpVid;
        }
        else if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[2])
        {
            Debug.Log("EN");
            videoPlayer.clip = enVid;
        }

        videoPlayer.Play();
        videoPlayer.playbackSpeed = 1;

        videoPlayer.loopPointReached += EndReached;
    }

    private void Start()
    {
        LoadSceneManager.Instance.LoadingObj = loadingObj;
    }

    private void Update()
    {
        if(inputSystemUIInputModule.cancel.action.triggered && !skipped)
        {
            skipped = true;
            DataPersistenceManager.Instance.ReloadBaseScene();
        }
        
        if(inputSystemUIInputModule.leftClick.action.triggered)
        {
            if(videoPlayer.playbackSpeed == 1)
            {
                videoPlayer.playbackSpeed = 2;
            }
            else
            {
                videoPlayer.playbackSpeed = 1;
            }
        }
    }

    private void EndReached(VideoPlayer vp)
    {
        videoPlayer.playbackSpeed = 1;
        DataPersistenceManager.Instance.ReloadBaseScene();
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= EndReached;
    }
}
