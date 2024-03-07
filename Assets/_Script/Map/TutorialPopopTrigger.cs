using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class TutorialPopopTrigger : DataPersistMapObjBase
{
    [SerializeField] private LocalizedString[] popupStings;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            foreach (var item in popupStings)
            {
                UI_Manager.Instance.ActivateTutorialPopUpUI(item);
            }

            gameObject.SetActive(false);
            Invoke(nameof(SaveData), 0.15f);
        }
    }
    protected override void Start()
    {
        base.Start();

        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }
    private void SaveData()
    {
        CancelInvoke(nameof(SaveData));
        DataPersistenceManager.Instance.SaveGame(false);
    }
}
