using TMPro;
using UnityEngine;

public class SavepointUIMain : MonoBehaviour
{
    [SerializeField] private GameObject savepointUIObj;
    [SerializeField] private TextMeshProUGUI savepointNameText;
    [SerializeField] private SavepointUIInventory savepointUIInventory;


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

    public void ActiveMenu(bool init = false)
    {
        if (init)
        {
            savepointUIObj.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
    }

    public void DeactiveAllMenu()
    {
        savepointUIInventory.DeactiveMenu();
        savepointUIObj.SetActive(false);
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
}
