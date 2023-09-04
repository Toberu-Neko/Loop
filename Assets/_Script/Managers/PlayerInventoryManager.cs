using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IDataPersistance
{
    public static PlayerInventoryManager Instance { get; private set; }
    public SerializableDictionary<string, ItemData> Inventory { get; private set; }

    public MultiplierData SwordMultiplier { get; private set; }
    public MultiplierData GunMultiplier { get; private set; }
    public MultiplierData FistMultiplier { get; private set; }

    private List<EquipedItem> equipedItems;
    private class EquipedItem
    {
        public WeaponType equipmentType;
        public LootSO lootSO;

        public EquipedItem(WeaponType equipmentType, LootSO SO)
        {
            this.equipmentType = equipmentType;
            lootSO = SO;
        }
    }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Inventory = new();
        equipedItems = new();
        SwordMultiplier = new();
        GunMultiplier = new();
        FistMultiplier = new();
    }

    public void AddItem(ItemDetails lootDetails, int amount = 1)
    {
        if (Inventory.ContainsKey(lootDetails.lootName))
        {
            Inventory[lootDetails.lootName].itemCount += amount;
        }
        else
        {
            Inventory.Add(lootDetails.lootName, new ItemData { lootDetails = lootDetails, itemCount = amount });
        }
        // Debug.Log("You have " + Inventory[lootDetails.lootName].itemCount + " " + lootDetails.lootName + " in your inventory.");
    }

    private void UpdateEquipedItem()
    {
        SwordMultiplier = new();
        GunMultiplier = new();
        FistMultiplier = new();

        foreach (var item in equipedItems)
        {
            switch (item.equipmentType)
            {
                case WeaponType.Sword:
                    SwordMultiplier.attackSpeedMultiplier += item.lootSO.multiplierData.attackSpeedMultiplier / 100f;
                    SwordMultiplier.damageMultiplier += item.lootSO.multiplierData.damageMultiplier / 100f;
                    break;
                case WeaponType.Gun:
                    GunMultiplier.attackSpeedMultiplier += item.lootSO.multiplierData.attackSpeedMultiplier / 100f;
                    GunMultiplier.damageMultiplier += item.lootSO.multiplierData.damageMultiplier / 100f;
                    break;
                case WeaponType.Fist:
                    FistMultiplier.attackSpeedMultiplier += item.lootSO.multiplierData.attackSpeedMultiplier / 100f;
                    FistMultiplier.damageMultiplier += item.lootSO.multiplierData.damageMultiplier / 100f;
                    break;
                default:
                    Debug.LogError("Equipment Type not found in " + gameObject.name + ".");
                    break;
            }
        }
        /*
        Debug.Log("SwordMultiplier: " + SwordMultiplier.damageMultiplier + " " + SwordMultiplier.attackSpeedMultiplier);
        Debug.Log("GunMultiplier: " + GunMultiplier.damageMultiplier + " " + GunMultiplier.attackSpeedMultiplier);
        Debug.Log("FistMultiplier: " + FistMultiplier.damageMultiplier + " " + FistMultiplier.attackSpeedMultiplier);
        */
    }

    public void EquipItem(LootSO SO, WeaponType equipmentType)
    {
        equipedItems.Add(new EquipedItem(equipmentType, SO));
        UpdateEquipedItem();
    }

    public void UnEquipItem(LootSO SO, WeaponType type)
    {
        equipedItems.Remove(equipedItems.Find(x => x.equipmentType == type && x.lootSO == SO));
        UpdateEquipedItem();
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
    public ItemDetails lootDetails;
}

