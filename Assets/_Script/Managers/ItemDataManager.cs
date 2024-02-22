using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance { get; private set; }

    public Dictionary<string, SO_Chip> ChipDict { get; private set; }
    public Dictionary<string, SO_MovementSkillItem> MovementSkillDict { get; private set; }
    public Dictionary<string, SO_TimeSkillItem> TimeSkillDict { get; private set; }
    public Dictionary<string, SO_PlayerStatusEnhancement> StatusEnhancementDict { get; private set; }
    public Dictionary<string, SO_StoryItem> StoryItemDict { get; private set; }
    public Dictionary<string, SO_ConsumeableItem> ConsumableItemDict { get; private set; }
    public Dictionary<string, SO_WeaponItem> WeaponItemDict { get; private set; }
    public Dictionary<string, SO_Savepoint> SavepointDict { get; private set; }
    public Dictionary<string, SO_Shop> ShopDict { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ChipDict = new();
        MovementSkillDict = new();
        TimeSkillDict = new();
        StatusEnhancementDict = new();
        StoryItemDict = new();
        ConsumableItemDict = new();
        WeaponItemDict = new();
        SavepointDict = new();
        ShopDict = new();

        LoadAndPopulateDictionary("SO_Chip", ChipDict);
        LoadAndPopulateDictionary("SO_MovementSkill", MovementSkillDict);
        LoadAndPopulateDictionary("SO_TimeSkill", TimeSkillDict);
        LoadAndPopulateDictionary("SO_StatusEnhancement", StatusEnhancementDict);
        LoadAndPopulateDictionary("SO_StoryItem", StoryItemDict);
        LoadAndPopulateDictionary("SO_ConsumableItem", ConsumableItemDict);
        LoadAndPopulateDictionary("SO_WeaponItem", WeaponItemDict);

        LoadAndPopulateSavepointDictionary("SO_Savepoint", SavepointDict);

        
        LoadAndPopulateShopDictionary("SO_Shop", ShopDict);
    }

    public SO_ItemsBase TryGetItemFromAllDict(string id)
    {
        SO_ItemsBase item = null;

        item = CheckDict(ChipDict, id, item);
        item = CheckDict(MovementSkillDict, id, item);
        item = CheckDict(TimeSkillDict, id, item);
        item = CheckDict(StatusEnhancementDict, id, item);
        item = CheckDict(StoryItemDict, id, item);
        item = CheckDict(ConsumableItemDict, id, item);
        item = CheckDict(WeaponItemDict, id, item);

        if (item == null)
        {
            Debug.LogError($"Item with id: {id} not found");
        }
        return item;
    }

    private SO_ItemsBase CheckDict<T>(Dictionary<string, T> dict, string id, SO_ItemsBase currentItem) where T : SO_ItemsBase
    {
        if (dict.TryGetValue(id, out var tempItem))
        {
            if (currentItem != null)
            {
                Debug.LogError($"There are more than one item with the same name: {id}");
            }
            return tempItem;
        }
        return currentItem;
    }


    private void LoadAndPopulateDictionary<T>(string folderName, Dictionary<string, T> targetDict) where T : SO_ItemsBase
    {
        var items = Resources.LoadAll<T>(folderName);

        foreach (var item in items)
        {

            if (string.IsNullOrEmpty(item.ID))
            {
                Debug.LogError($"There is an item with no name, name: {item.ID}");
                continue;
            }

            if (targetDict.ContainsKey(item.ID))
            {
                Debug.LogError($"There are more than one item with the same name: {item.ID}");
                continue;
            }

            targetDict.Add(item.ID, item);
        }
    }

    private void LoadAndPopulateSavepointDictionary<T>(string folderName, Dictionary<string, T> targetDict) where T : SO_Savepoint
    {
        var items = Resources.LoadAll<T>(folderName);

        foreach (var item in items)
        {

            if (string.IsNullOrEmpty(item.savepointID))
            {
                Debug.LogError($"There is an savepoint with no ID, ID: {item.savepointID}");
                continue;
            }

            if (targetDict.ContainsKey(item.savepointID))
            {
                Debug.LogError($"There are more than one item with the same ID: {item.savepointID}");
                continue;
            }

            targetDict.Add(item.savepointID, item);
        }
    }

    private void LoadAndPopulateShopDictionary<T>(string folderName, Dictionary<string, T> targetDict) where T : SO_Shop
    {
        var items = Resources.LoadAll<T>(folderName);

        foreach (var item in items)
        {

            if (string.IsNullOrEmpty(item.shopID))
            {
                Debug.LogError($"There is an savepoint with no ID, ID: {item.shopID}");
                continue;
            }

            if (targetDict.ContainsKey(item.shopID))
            {
                Debug.LogError($"There are more than one item with the same ID: {item.shopID}");
                continue;
            }

            targetDict.Add(item.shopID, item);
        }
    }
}

public enum ItemType
{
    Chip,
    MovementSkill,
    TimeSkill,
    StatusEnhancement,
    StoryItem,
    ConsumableItem,
    WeaponItem
}
