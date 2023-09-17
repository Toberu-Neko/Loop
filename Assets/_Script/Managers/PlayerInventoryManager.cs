using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IDataPersistance
{
    public static PlayerInventoryManager Instance { get; private set; }
    public int Money { get; private set; }
    public event Action OnMoneyChanged;

    public SerializableDictionary<string, ItemData> StatusEnhancementInventory { get; private set; }
    // public SerializableDictionary<string, ItemData> StoryItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> ChipInventory { get; private set; }

    public WeaponType[] EquipedWeapon { get; private set; }

    public MultiplierData SwordMultiplier { get; private set; }
    public MultiplierData GunMultiplier { get; private set; }
    public MultiplierData FistMultiplier { get; private set; }

    private List<EquipedItem> equipedItems;
    private class EquipedItem
    {
        public WeaponType equipmentType;
        public SO_Chip lootSO;

        public EquipedItem(WeaponType equipmentType, SO_Chip SO)
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

        equipedItems = new();
        SwordMultiplier = new();
        GunMultiplier = new();
        FistMultiplier = new();
    }

    private void Start()
    {
        if (DataPersistenceManager.Instance.DisableDataPersistance)
        {
            Debug.LogError("Data persistance is disabled, so player can't change weapon.");
            EquipedWeapon = new WeaponType[2];
            EquipedWeapon[0] = WeaponType.Sword;
            EquipedWeapon[1] = WeaponType.Gun;

            Debug.LogError("Data persistance is disabled, so player can't save picked items.");
            ChipInventory = new();
            equipedItems = new();
            Money = 99999999;
        }
    }

    #region PlayerStatusEnhancementInventory

    public void AddPlayerStatusEnhancementItem(ItemDetails lootDetails, int amount = 1)
    {
        if (StatusEnhancementInventory.ContainsKey(lootDetails.lootName))
        {
            StatusEnhancementInventory[lootDetails.lootName].itemCount += amount;
        }
        else
        {
            StatusEnhancementInventory.Add(lootDetails.lootName, new ItemData { lootDetails = lootDetails, itemCount = amount });
        }
    }

    #endregion

    #region Weapon
    public void ChangeEquipWeapon1(WeaponType type)
    {
        EquipedWeapon[0] = type;
    }

    public void ChangeEquipWeapon2(WeaponType type)
    {
        EquipedWeapon[1] = type;
    }
    #endregion

    #region Chip
    public void AddChip(ItemDetails lootDetails, int amount = 1)
    {
        if (ChipInventory.ContainsKey(lootDetails.lootName))
        {
            ChipInventory[lootDetails.lootName].itemCount += amount;
        }
        else
        {
            ChipInventory.Add(lootDetails.lootName, new ItemData { lootDetails = lootDetails, itemCount = amount });
        }
    }

    private void UpdateEquipedChips()
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
    }

    public void EquipChip(SO_Chip SO, WeaponType equipmentType)
    {
        equipedItems.Add(new EquipedItem(equipmentType, SO));
        UpdateEquipedChips();
    }

    public void UnEquipChip(SO_Chip SO, WeaponType type)
    {
        equipedItems.Remove(equipedItems.Find(x => x.equipmentType == type && x.lootSO == SO));
        UpdateEquipedChips();
    }

    #endregion

    #region Money

    public void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke();
    }

    public void RemoveMoney(int amount)
    {
        Money -= amount;
        OnMoneyChanged?.Invoke();
    }
    #endregion

    public void LoadData(GameData data)
    {
        ChipInventory = new();
        StatusEnhancementInventory = new();

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
        ChipInventory = data.chipInventory;
        Money = data.money;
        StatusEnhancementInventory = data.statusEnhancementInventory;
    }

    public void SaveData(GameData data)
    {
        if (ChipInventory != null)
        {
            data.chipInventory = ChipInventory;
        }

        if(EquipedWeapon != null)
        {
            data.equipedWeapon = EquipedWeapon;
        }

        data.money = Money;
        data.statusEnhancementInventory = StatusEnhancementInventory;
    }
}

[System.Serializable]
public class ItemData
{
    public int itemCount;
    public ItemDetails lootDetails;
}

