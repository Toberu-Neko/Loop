using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for managing all the items in the game.
/// Loads all items from the resources folder and populates the dictionaries when first time opened the game.
/// </summary>
public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance { get; private set; }

    #region Dictionaries
    public Dictionary<string, SO_Chip> ChipDict { get; private set; }
    public Dictionary<string, SO_MovementSkillItem> MovementSkillDict { get; private set; }
    public Dictionary<string, SO_TimeSkillItem> TimeSkillDict { get; private set; }
    public Dictionary<string, SO_PlayerStatusEnhancement> StatusEnhancementDict { get; private set; }
    public Dictionary<string, SO_StoryItem> StoryItemDict { get; private set; }
    public Dictionary<string, SO_ConsumeableItem> ConsumableItemDict { get; private set; }
    public Dictionary<string, SO_WeaponItem> WeaponItemDict { get; private set; }
    public Dictionary<string, SO_Savepoint> SavepointDict { get; private set; }
    public Dictionary<string, SO_Shop> ShopDict { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        #region Initialize Dictionaries
        ChipDict = new();
        MovementSkillDict = new();
        TimeSkillDict = new();
        StatusEnhancementDict = new();
        StoryItemDict = new();
        ConsumableItemDict = new();
        WeaponItemDict = new();
        SavepointDict = new();
        ShopDict = new();
        #endregion

        LoadAndPopulateItemDictionary("SO_Chip", ChipDict);
        LoadAndPopulateItemDictionary("SO_MovementSkill", MovementSkillDict);
        LoadAndPopulateItemDictionary("SO_TimeSkill", TimeSkillDict);
        LoadAndPopulateItemDictionary("SO_StatusEnhancement", StatusEnhancementDict);
        LoadAndPopulateItemDictionary("SO_StoryItem", StoryItemDict);
        LoadAndPopulateItemDictionary("SO_ConsumableItem", ConsumableItemDict);
        LoadAndPopulateItemDictionary("SO_WeaponItem", WeaponItemDict);

        // Savepoint and Shop dictionaries are different from the item dictionaries.
        LoadAndPopulateSavepointDictionary("SO_Savepoint", SavepointDict);
        LoadAndPopulateShopDictionary("SO_Shop", ShopDict);
    }

    /// <summary>
    /// Provide item ID and get the item from the dictionary.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Use item ID to get the item from the dictionary, and check if the item is already found in another dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <param name="id"></param>
    /// <param name="currentItem">Check if the item is already found in another dictionary</param>
    /// <returns></returns>
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

    private void LoadAndPopulateItemDictionary<T>(string folderName, Dictionary<string, T> targetDict) where T : SO_ItemsBase
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
