using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavepointUIMain : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI savepointNameText;
    [SerializeField] private SavepointUIInventory savepointUIInventory;
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
        DeactiveAllMenu();
    }

    public void OnClickInventoryButton()
    {
        savepointUIInventory.ActiveMenu();
        DeactiveMenu();
    }

    public void ActiveMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
    }

    public void DeactiveAllMenu()
    {
        gameObject.SetActive(false);
        savepointUIInventory.DeactiveMenu();
        gameManager.ResumeGame();
    }
}
