using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int debugInputCount;

    public float maxHealth;
    public float timePlayed;

    public Vector3 playerPos;

    // public SerializableDictionary<string, bool> defeatedEnemies;
    public SerializableDictionary<string, bool> activatedSavepoints;
    public SerializableDictionary<string, ItemData> inventory;
    public SerializableDictionary<string, string> equipedItems;

    public GameData()
    {
        maxHealth = 100f;
        debugInputCount = 0;
        timePlayed = 0f;
        playerPos = Vector3.zero;

        activatedSavepoints = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, ItemData>();
        equipedItems = new();
        // defeatedEnemies = new SerializableDictionary<string, bool>();
    }

    public int GetPercentageComplete()
    {
        //TODO: use save points to calculate percentage complete
        return 24;
    }
}
