using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Video;

public class TutorialUIOpenObj : DataPersistMapObjBase
{
    [SerializeField] private SO_Tutorial so;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            UI_Manager.Instance.ActiveTutorialUI(so.clip, so.title, so.description);

            gameObject.SetActive(false);
            DataPersistenceManager.Instance.SaveGame(so.returnToSavepoint);
        }
    }
}
