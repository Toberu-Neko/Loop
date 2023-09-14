using System;
using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour, IDataPersistance
{
    // Teleport


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
        data.playerPos = transform.position;
    }
}
