using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class TutorialPopopTrigger : DataPersistMapObjBase
{
    [SerializeField] private LocalizedString[] popupStings;
    [SerializeField] private bool returnToSavepoint = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            if(popupStings != null)
            {
                if (popupStings.Length > 0)
                {
                    foreach (var item in popupStings)
                    {
                        UI_Manager.Instance.ActivateTutorialPopUpUI(item);
                    }
                }
            }

            gameObject.SetActive(false);
            DataPersistenceManager.Instance.SaveGame(returnToSavepoint);
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
}
