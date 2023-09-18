using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IDataPersistance
{
    public static PlayerInventoryManager Instance { get; private set; }
    public int Money { get; private set; }
    public event Action OnMoneyChanged;

    public SerializableDictionary<string, ItemData> StatusEnhancementInventory { get; private set; }
    public SerializableDictionary<string, ItemData> StoryItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> MovementSkillItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> TimeSkillItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> ConsumablesInventory { get; private set; }
    public SerializableDictionary<string, ItemData> ChipInventory { get; private set; }

    #region Weapon Variables
    public WeaponType[] EquipedWeapon { get; private set; }

    public MultiplierData SwordMultiplier { get; private set; }
    public MultiplierData GunMultiplier { get; private set; }
    public MultiplierData FistMultiplier { get; private set; }

    #endregion

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
        ConsumablesInventory = new()
        {
            { "Medkit", new ItemData(3, "Medkit") }
        };
        StatusEnhancementInventory = new();
        MovementSkillItemInventory = new();
        TimeSkillItemInventory = new();
        StoryItemInventory = new();
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
            StatusEnhancementInventory = new();
            ConsumablesInventory = new();
            Money = 99999999;
        }
    }

    public void AddPlayerStatusEnhancementItem(string name, int amount = 1)
    {
        if (StatusEnhancementInventory.ContainsKey(name))
        {
            StatusEnhancementInventory[name].IncreaseItemCount(amount);
        }
        else
        {
            StatusEnhancementInventory.Add(name, new ItemData(amount, name));
        }
    }

    public void AddMovemnetSkillItem(string name)
    {
        if(MovementSkillItemInventory.ContainsKey(name))
        {
            MovementSkillItemInventory[name].ItemCount = 1;
        }
        else
        {
            MovementSkillItemInventory.Add(name, new ItemData(1, name));
        }
    }

    public void AddTimeSkillItem(string name)
    {
        if (TimeSkillItemInventory.ContainsKey(name))
        {
            TimeSkillItemInventory[name].ItemCount = 1;
        }
        else
        {
            TimeSkillItemInventory.Add(name, new ItemData(1, name));
        }
    }

    public void AddStoryItem(string name)
    {
        if (StoryItemInventory.ContainsKey(name))
        {
            StoryItemInventory[name].ItemCount = 1;
        }
        else
        {
            StoryItemInventory.Add(name, new ItemData(1, name));
        }
    }

    #region ConsumableItem
    public void AddConsumableItem(ItemDetails lootDetails, int amount = 1)
    {
        if (ConsumablesInventory.ContainsKey(lootDetails.lootName))
        {
            ConsumablesInventory[lootDetails.lootName].IncreaseItemCount(amount);
        }
        else
        {
            ConsumablesInventory.Add(lootDetails.lootName, new ItemData(amount, lootDetails.lootName));
        }
    }


    public void RemoveConsumableItem(string name, int amount = 1)
    {
        if (ConsumablesInventory.ContainsKey(name))
        {
            if (ConsumablesInventory[name].ItemCount > 0)
            {
                ConsumablesInventory[name].ReduceItemCount(amount);
            }
            else
            {
                Debug.LogError(name + " item count is less or equal to zero, and trying to reduce the amount of it.");
            }

            if (ConsumablesInventory[name].ItemCount < 0)
            {
                Debug.LogError("Item count is less than zero.");
            }
        }
        else
        {
            Debug.LogError("Item not found in inventory.");
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
    public void AddChip(string name, int amount = 1)
    {
        if (ChipInventory.ContainsKey(name))
        {
            ChipInventory[name].IncreaseItemCount(amount);
        }
        else
        {
            ChipInventory.Add(name, new ItemData (amount, name));
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
        ConsumablesInventory = data.consumablesInventory;
        TimeSkillItemInventory = data.timeSkillItemInventory;
        MovementSkillItemInventory = data.movementSkillItemInventory;
        StoryItemInventory = data.storyItemInventory;
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
        data.consumablesInventory = ConsumablesInventory;
        data.timeSkillItemInventory = TimeSkillItemInventory;
        data.movementSkillItemInventory = MovementSkillItemInventory;
        data.storyItemInventory = StoryItemInventory;
    }
}

[System.Serializable]
public class ItemData
{
    public int ItemCount;
    public ItemDetails lootDetails;
    public event Action OnValueChanged;

    public ItemData(int count, string name)
    {
        ItemCount = count;
        lootDetails = new(name);
    }

    public void ReduceItemCount(int amount)
    {
        ItemCount -= amount;
        OnValueChanged?.Invoke();
    }

    public void IncreaseItemCount(int amount)
    {
        ItemCount += amount;
        OnValueChanged?.Invoke();
    }
}

