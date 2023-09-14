using System;
using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour, IDataPersistance
{
    // Teleport, Money
    public int Money { get; private set; } = 0;

    public event Action OnMoneyChanged;

    public void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke();
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
        data.playerPos = transform.position;
    }
}
