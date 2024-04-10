using UnityEngine;

public class TutorialUIOpenObj : DataPersistMapObjBase
{
    [SerializeField] private SO_Tutorial so;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            UI_Manager.Instance.ActiveTutorialUI(so.clip, so.title, so.description);

            if (so.giveStoryItem)
            {
                PlayerInventoryManager.Instance.AddStoryItem(so.giveStoryItem.ID);
            }

            gameObject.SetActive(false);
            DataPersistenceManager.Instance.SaveGame(so.returnToSavepoint);
        }
    }
}
