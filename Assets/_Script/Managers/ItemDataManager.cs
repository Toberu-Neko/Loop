using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance { get; private set; }

    public Dictionary<string, LootSO> LootSODict { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        var lootSOs = Resources.LoadAll<LootSO>("LootSO");
        LootSODict = new();

        foreach (var item in lootSOs)
        {
            if(LootSODict.ContainsKey(item.itemDetails.lootName))
            {
                Debug.LogError($"There are more than one loot with the same name: {item.name}");
                continue;
            }

            if(item.itemDetails.lootName == "")
            {
                Debug.LogError($"There is a loot with no name, name: " + item.name);
                continue;
            }

            LootSODict.Add(item.itemDetails.lootName, item);
        }
    }
}
