using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavepointUITeleport : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;
    public void OnClickBackButton()
    {
        Deactivate();
    }

    private void UpdateButtons()
    {
        int count = 0;
        foreach(var item in GameManager.Instance.Savepoints)
        {
            var buttonObj = ObjectPoolManager.SpawnObject(buttonPrefab, buttonParent);
            var script = buttonObj.GetComponent<TeleportButton>();
            script.SetText(item.Value.SavePointData.savepointID, item.Value.SavePointData.savepointName);

            count++;
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        UpdateButtons();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
