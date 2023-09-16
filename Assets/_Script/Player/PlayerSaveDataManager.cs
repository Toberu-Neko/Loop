using System;
using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour, IDataPersistance
{
    // Teleport

    private string recentSavepointName = "";

    private void Start()
    {
        GameManager.Instance.OnSavepointInteracted += HandleSavepointInteract;
    }

    private void HandleSavepointInteract(string name)
    {
        recentSavepointName = name;
    }

    public void LoadData(GameData data)
    {
        data.savepoints.TryGetValue(data.lastInteractedSavepoint, out SavepointDetails details);

        if (details != null)
        {
            transform.position = details.teleportPosition;
        }
    }

    public void SaveData(GameData data)
    {
        data.lastInteractedSavepoint = recentSavepointName;
    }
}
