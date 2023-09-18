using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance { get; private set; }

    public Dictionary<string, SO_Chip> LootSODict { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        var lootSOs = Resources.LoadAll<SO_Chip>("LootSO");
        LootSODict = new();

        foreach (var item in lootSOs)
        {
            if(LootSODict.ContainsKey(item.itemName))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if(item.itemName == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            LootSODict.Add(item.itemName, item);
        }
    }
}
