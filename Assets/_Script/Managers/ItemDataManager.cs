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

        var chipSOs = Resources.LoadAll<SO_Chip>("ChipSO");
        ChipDict = new();

        foreach (var item in chipSOs)
        {
            if (ChipDict.ContainsKey(item.itemName))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if (item.itemName == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            ChipDict.Add(item.itemName, item);
        }

        var movementSkillSOs = Resources.LoadAll<SO_MovementSkillItem>("MovementSkillSO");
        MovementSkillDict = new();

        foreach (var item in movementSkillSOs)
        {
            if (MovementSkillDict.ContainsKey(item.name))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if (item.name == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            MovementSkillDict.Add(item.name, item);
        }

        var timeSkillSOs = Resources.LoadAll<SO_TimeSkillItem>("TimeSkillSO");
        TimeSkillDict = new();

        foreach (var item in timeSkillSOs)
        {
            if (TimeSkillDict.ContainsKey(item.name))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if (item.name == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            TimeSkillDict.Add(item.name, item);
        }

        var statusEnhancementSOs = Resources.LoadAll<SO_PlayerStatusEnhancement>("StatusEnhancementSO");
        StatusEnhancementDict = new();

        foreach (var item in statusEnhancementSOs)
        {
            if (StatusEnhancementDict.ContainsKey(item.name))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if (item.name == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            StatusEnhancementDict.Add(item.name, item);
        }

        var storyItemSOs = Resources.LoadAll<SO_StoryItem>("StoryItemSO");
        StoryItemDict = new();

        foreach (var item in storyItemSOs)
        {
            if (StoryItemDict.ContainsKey(item.name))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if (item.name == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            StoryItemDict.Add(item.name, item);
        }

        var consumableItemSOs = Resources.LoadAll<SO_ConsumeableItem>("ConsumableItemSO");
        ConsumableItemDict = new();

        foreach (var item in consumableItemSOs)
        {
            if (ConsumableItemDict.ContainsKey(item.name))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if (item.name == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            ConsumableItemDict.Add(item.name, item);
        }

        Debug.Log(ConsumableItemDict.Count);
        Debug.Log(StoryItemDict.Count);
        Debug.Log(StatusEnhancementDict.Count);
        Debug.Log(TimeSkillDict.Count);
        Debug.Log(MovementSkillDict.Count);
        Debug.Log(ChipDict.Count);

    }
}
