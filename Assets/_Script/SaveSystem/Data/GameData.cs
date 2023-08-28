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

    // public SerializableDictionary<string, bool> defeatedEnemies;
    public SerializableDictionary<string, bool> activatedSavepoints;

    public GameData()
    {
        maxHealth = 100f;
        debugInputCount = 0;
        timePlayed = 0f;

        activatedSavepoints = new SerializableDictionary<string, bool>();
        // defeatedEnemies = new SerializableDictionary<string, bool>();
    }

    public int GetPercentageComplete()
    {
        //TODO: use save points to calculate percentage complete
        return 24;
    }
}
