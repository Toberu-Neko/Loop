using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseTeleport : MonoBehaviour
{
    [SerializeField] private PauseUIMain pauseUIMain;
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> teleports;
    [Serializable] private class Teleport
    {
        public string levelName;
        public Transform teleportTransform;
    }


    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        pauseUIMain.ActivateMenu();
    }

    public void OnClickTeleport(string levelName)
    {
        foreach (var teleport in teleports)
        {
            if(teleport.name == levelName)
            {
                UI_Manager.Instance.HandleChangeSceneGoRight();
                pauseUIMain.DeactiveAllMenu();
                if(SceneManager.GetSceneByName(LoadSceneManager.Instance.CurrentSceneName).isLoaded)
                    SceneManager.UnloadSceneAsync(LoadSceneManager.Instance.CurrentSceneName);
                player.HandleChangeSceneToRight();
                player.transform.position = teleport.position;
                return;
            }
        }
    }
}
