using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IDataPersistance
{
    public static PlayerInventoryManager Instance { get; private set; }
    public SerializableDictionary<string, ItemData> Inventory { get; private set; }
    public WeaponType[] EquipedWeapon { get; private set; }

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

        SwordMultiplier = new();
        GunMultiplier = new();
        FistMultiplier = new();
    }

    public void ChangeEquipWeapon1(WeaponType type)
    {
        EquipedWeapon[0] = type;
    }

    public void ChangeEquipWeapon2(WeaponType type)
    {
        EquipedWeapon[1] = type;
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
        Inventory = new();
        equipedItems = new();

        if (data.equipedWeapon.Length == 0)
        {
            EquipedWeapon = new WeaponType[2];
            EquipedWeapon[0] = WeaponType.Sword;
            EquipedWeapon[1] = WeaponType.Gun;
        }
        else
        {
            EquipedWeapon = data.equipedWeapon;
        }
        Debug.Log("LoadData in inventorymanager = inventory: " + Inventory.Count + "& data: " + data.inventory.Count);
        Inventory = data.inventory;
    }

    public void SaveData(GameData data)
    {
        if (Inventory != null && EquipedWeapon != null)
        {
            data.inventory = Inventory;
            data.equipedWeapon = EquipedWeapon;
            Debug.Log("SaveData in inventorymanager = " + data.inventory.Count);
        }
    }
}

[System.Serializable]
public class ItemData
{
    public int itemCount;
    public ItemDetails lootDetails;
}

