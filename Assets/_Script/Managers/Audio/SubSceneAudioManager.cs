using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneAudioManager : MonoBehaviour
{
    [SerializeField] private string bgmName;
    [SerializeField] private bool stopBGMOnLoad = false;

    private void Start()
    {
        if (stopBGMOnLoad)
        {
            AudioManager.Instance.StopBGM(bgmName, 1f);
        }

        if(bgmName != "")
        {
            AudioManager.Instance.PlayBGM(bgmName);
        }
    }
}
