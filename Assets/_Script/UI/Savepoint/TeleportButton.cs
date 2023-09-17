using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;
    public string SavePointName { get; set; }

    private void OnEnable()
    {
        button.interactable = false;
    }

    public void OnClick()
    {
        PlayerSaveDataManager.Instance.RecentSavepointName = SavePointName;
        UI_Manager.Instance.CloseAllSavePointUI();
    }

    public void SetText(string text)
    {
        SavePointName = text;
        buttonText.text = text;
        DataPersistenceManager.Instance.GameData.savepoints.TryGetValue(text, out SavepointDetails details);

        if (details.isActivated)
        {
            button.interactable = true;
        }
    }
}
