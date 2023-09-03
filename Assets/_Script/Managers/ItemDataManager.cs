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
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        var lootSOs = Resources.LoadAll<LootSO>("LootSO");
        LootSODict = new();
        foreach (var item in lootSOs)
        {
            LootSODict.Add(item.lootDetails.lootName, item);
        }
    }
}
