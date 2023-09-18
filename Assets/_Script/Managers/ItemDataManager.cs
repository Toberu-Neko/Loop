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

        LoadAndPopulateDictionary("ChipSO", ChipDict);
        LoadAndPopulateDictionary("MovementSkillSO", MovementSkillDict);
        LoadAndPopulateDictionary("TimeSkillSO", TimeSkillDict);
        LoadAndPopulateDictionary("StatusEnhancementSO", StatusEnhancementDict);
        LoadAndPopulateDictionary("StoryItemSO", StoryItemDict);
        LoadAndPopulateDictionary("ConsumableItemSO", ConsumableItemDict);

        Debug.Log(ConsumableItemDict.Count);
        Debug.Log(StoryItemDict.Count);
        Debug.Log(StatusEnhancementDict.Count);
        Debug.Log(TimeSkillDict.Count);
        Debug.Log(MovementSkillDict.Count);
        Debug.Log(ChipDict.Count);

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
}
