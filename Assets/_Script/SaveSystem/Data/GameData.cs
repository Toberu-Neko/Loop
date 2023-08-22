using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int debugInputCount;

    public float maxHealth;

    public SerializableDictionary<string, bool> defeatedEnemies;

    public GameData()
    {
        maxHealth = 100f;
        debugInputCount = 0;
        defeatedEnemies = new SerializableDictionary<string, bool>();
    }
}
