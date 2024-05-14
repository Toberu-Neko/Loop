using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;
    [SerializeField] private LocalizeStringEvent localizeStringEvent;
    private string savePointID;

    private void OnEnable()
    {
        button.interactable = false;
        buttonText.color = Color.gray;
    }

    public void OnClick()
    {
        PlayerSaveDataManager.Instance.RecentSavepointID = savePointID;
        UI_Manager.Instance.CloseAllSavePointUI();
    }

    public void SetText(string savepointID, LocalizedString text)
    {
        savePointID = savepointID;
        localizeStringEvent.StringReference = text;
        DataPersistenceManager.Instance.GameData.savepoints.TryGetValue(savepointID, out SavepointDetails details);

        if (details.isActivated)
        {
            button.interactable = true;
            buttonText.color = Color.white;
        }
    }
}
