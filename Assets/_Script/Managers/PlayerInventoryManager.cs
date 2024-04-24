using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IDataPersistance
{
    public static PlayerInventoryManager Instance { get; private set; }
    public int Money { get; private set; }
    public event Action OnMoneyChanged;

    public event Action OnTimeSkillChanged;
    public SerializableDictionary<string, ItemData> StatusEnhancementInventory { get; private set; }
    public SerializableDictionary<string, ItemData> StoryItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> WeaponInventory { get; private set; }
    public SerializableDictionary<string, ItemData> MovementSkillItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> TimeSkillItemInventory { get; private set; }
    public SerializableDictionary<string, ItemData> ConsumablesInventory { get; private set; }
    public SerializableDictionary<string, ItemData> ChipInventory { get; private set; }

    #region Weapon Variables
    public WeaponType[] EquipedWeapon { get; private set; }
    public bool CanUseSword { get; private set; }
    public bool CanUseGun { get; private set; }
    public bool CanUseFist { get; private set; }
    public int CanUseWeaponCount = 0;

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
        WeaponInventory = new();
    }

    private void Start()
    {
        if (DataPersistenceManager.Instance.DisableDataPersistance)
        {
            Debug.LogError("Data persistance is disabled, so player can't change weapon.");
            CanUseWeaponCount = 2;
            EquipedWeapon = new WeaponType[2];
            EquipedWeapon[0] = WeaponType.Sword;
            EquipedWeapon[1] = WeaponType.Gun;

            Debug.LogError("Data persistance is disabled, so player can't save picked items.");
            MovementSkillItemInventory = new();
            TimeSkillItemInventory = new();
            StoryItemInventory = new();
            ChipInventory = new();
            equipedItems = new();
            StatusEnhancementInventory = new();
            ConsumablesInventory = new();
            WeaponInventory = new();
            Money = 99999999;
        }
    }

    public void AddItemByID(string id, int amount = 1)
    {
        var item = ItemDataManager.Instance.TryGetItemFromAllDict(id);

        if (item is SO_PlayerStatusEnhancement)
        {
            AddPlayerStatusEnhancementItem(id, amount);
        }
        else if (item is SO_StoryItem)
        {
            AddStoryItem(id);
        }
        else if (item is SO_WeaponItem)
        {
            AddWeaponItem(id);
        }
        else if (item is SO_MovementSkillItem)
        {
            AddMovemnetSkillItem(id);
        }
        else if (item is SO_TimeSkillItem)
        {
            AddTimeSkillItem(id);
        }
        else if (item is SO_ConsumeableItem)
        {
            AddConsumableItem(id, amount);
        }
        else if (item is SO_Chip)
        {
            AddChip(id, amount);
        }
        else
        {
            Debug.LogError("Item not found in ItemDataManager.");
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

    public void AddWeaponItem(string name)
    {
        if(WeaponInventory.ContainsKey(name))
        {
            WeaponInventory[name].itemCount = 1;
            CanUseWeaponCount = WeaponInventory.Count;
        }
        else
        {
            WeaponInventory.Add(name, new ItemData(1, name));
            CanUseWeaponCount = WeaponInventory.Count;
        }

        if (ItemDataManager.Instance.WeaponItemDict.TryGetValue(name, out SO_WeaponItem weaponItem))
        {
            CanUseSword |= weaponItem.unlockSword;
            CanUseGun |= weaponItem.unlockGun;
            CanUseFist |= weaponItem.unlockFist;
        }

        if (CanUseWeaponCount == 2)
        {
            EquipedWeapon = new WeaponType[2];
            EquipedWeapon[0] = WeaponType.Sword;

            if(CanUseGun)
            {
                EquipedWeapon[1] = WeaponType.Gun;
            }
            else if(CanUseFist)
            {
                EquipedWeapon[1] = WeaponType.Fist;
            }
        }
    }

    public void AddMovemnetSkillItem(string name)
    {
        if(MovementSkillItemInventory.ContainsKey(name))
        {
            MovementSkillItemInventory[name].itemCount = 1;
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
            TimeSkillItemInventory[name].itemCount = 1;
        }
        else
        {
            TimeSkillItemInventory.Add(name, new ItemData(1, name));
        }

        OnTimeSkillChanged?.Invoke();
    }

    public void AddStoryItem(string name)
    {
        if (StoryItemInventory.ContainsKey(name))
        {
            StoryItemInventory[name].itemCount = 1;
        }
        else
        {
            StoryItemInventory.Add(name, new ItemData(1, name));
        }
    }

    #region ConsumableItem
    public void AddConsumableItem(string name, int amount = 1)
    {
        if (ConsumablesInventory.ContainsKey(name))
        {
            ConsumablesInventory[name].IncreaseItemCount(amount);
        }
        else
        {
            ConsumablesInventory.Add(name, new ItemData(amount, name));
        }
    }


    public void RemoveConsumableItem(string name, int amount = 1)
    {
        if (ConsumablesInventory.ContainsKey(name))
        {
            if (ConsumablesInventory[name].itemCount > 0)
            {
                ConsumablesInventory[name].ReduceItemCount(amount);
                Debug.Log(ConsumablesInventory[name].itemCount);
            }
            else
            {
                Debug.LogError(name + " item count is less or equal to zero, and trying to reduce the amount of it.");
            }

            if (ConsumablesInventory[name].itemCount < 0)
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

        EquipedWeapon = data.equipedWeapon;
        ChipInventory = data.chipInventory;
        Money = data.money;
        StatusEnhancementInventory = data.statusEnhancementInventory;
        ConsumablesInventory = data.consumablesInventory;
        TimeSkillItemInventory = data.timeSkillItemInventory;
        MovementSkillItemInventory = data.movementSkillItemInventory;
        StoryItemInventory = data.storyItemInventory;
        WeaponInventory = data.weaponInventory;
        CanUseWeaponCount = 0;

        if (data.firstTimePlaying)
        {
            Debug.Log("First Load in Inventory!");

            CanUseSword = true;
            CanUseGun = false;
            CanUseFist = false;

            CanUseWeaponCount = 1;

            ChangeEquipWeapon1(WeaponType.Sword);
        }
        else
        {
            foreach (var item in WeaponInventory)
            {
                if (ItemDataManager.Instance.WeaponItemDict.TryGetValue(item.Key, out SO_WeaponItem weaponItem))
                {
                    CanUseSword |= weaponItem.unlockSword;
                    CanUseGun |= weaponItem.unlockGun;
                    CanUseFist |= weaponItem.unlockFist;
                }
                else
                {
                    Debug.LogError("The time skill item name in PlayerInventoryManager.Instance.TimeSkillItemInventory is wrong");
                }
            }

            CanUseWeaponCount = WeaponInventory.Count;
        }
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
        data.weaponInventory = WeaponInventory;
    }
}

[Serializable]
public class ItemData
{
    public int itemCount;
    public string itemdataID;
    public event Action OnValueChanged;

    public ItemData(int count, string name)
    {
        itemCount = count;
        itemdataID = new(name);
    }

    public void ReduceItemCount(int amount)
    {
        itemCount -= amount;
        OnValueChanged?.Invoke();
    }

    public void IncreaseItemCount(int amount)
    {
        itemCount += amount;
        OnValueChanged?.Invoke();
    }
}

