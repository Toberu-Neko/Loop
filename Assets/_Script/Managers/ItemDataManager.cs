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

        LoadAndPopulateDictionary("SO_Chip", ChipDict);
        LoadAndPopulateDictionary("SO_MovementSkill", MovementSkillDict);
        LoadAndPopulateDictionary("SO_TimeSkill", TimeSkillDict);
        LoadAndPopulateDictionary("SO_StatusEnhancement", StatusEnhancementDict);
        LoadAndPopulateDictionary("SO_StoryItem", StoryItemDict);
        LoadAndPopulateDictionary("SO_ConsumableItem", ConsumableItemDict);
        LoadAndPopulateDictionary("SO_WeaponItem", WeaponItemDict);

        LoadAndPopulateSavepointDictionary("SO_Savepoint", SavepointDict);
    }

    private void LoadAndPopulateDictionary<T>(string folderName, Dictionary<string, T> targetDict) where T : SO_ItemsBase
    {
        var items = Resources.LoadAll<T>(folderName);

        foreach (var item in items)
        {

            if (string.IsNullOrEmpty(item.itemName))
            {
                Debug.LogError($"There is an item with no name, name: {item.itemName}");
                continue;
            }

            if (targetDict.ContainsKey(item.itemName))
            {
                Debug.LogError($"There are more than one item with the same name: {item.itemName}");
                continue;
            }

            targetDict.Add(item.itemName, item);
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
