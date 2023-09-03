using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IDataPersistance
{
    public static PlayerInventoryManager Instance { get; private set; }
    public SerializableDictionary<string, ItemData> Inventory { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Inventory = new();
    }

    public void AddItem(LootDetails lootDetails, int amount = 1)
    {
        if (Inventory.ContainsKey(lootDetails.lootName))
        {
            Inventory[lootDetails.lootName].itemCount += amount;
        }
        else
        {
            Inventory.Add(lootDetails.lootName, new ItemData { lootDetails = lootDetails, itemCount = amount });
        }
        Debug.Log("You have " + Inventory[lootDetails.lootName].itemCount + " " + lootDetails.lootName + " in your inventory.");
    }

    public void LoadData(GameData data)
    {
        Inventory = data.inventory;
    }

    public void SaveData(GameData data)
    {
        data.inventory = Inventory;
    }
}

[System.Serializable]
public class ItemData
{
    public int itemCount;
    public LootDetails lootDetails;
}
