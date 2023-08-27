using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavepointUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI savepointNameText;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void SetSavepointNameText(string name)
    {
        savepointNameText.text = name;
    }

    public void OnClickSaveButton()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void OnClickBackButton()
    {
        DeactiveMenu();
    }

    public void ActiveMenu()
    {
        DataPersistenceManager.Instance.SaveGame();
        gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
        gameManager.ResumeGame();
    }
}
