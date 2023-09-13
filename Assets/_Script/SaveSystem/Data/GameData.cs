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

    public string currentTimeSkill;

    public SerializableDictionary<string, bool> defeatedBosses;
    public SerializableDictionary<string, bool> activatedSavepoints;
    public SerializableDictionary<string, ItemData> inventory;
    public SerializableDictionary<string, string> equipedItems;
    public WeaponType[] equipedWeapon = new WeaponType[2];

    public GameData()
    {
        maxHealth = 100f;
        debugInputCount = 0;
        timePlayed = 0f;
        playerPos = Vector3.zero;
        currentTimeSkill = "PlayerTimeSkill_None";

        activatedSavepoints = new();
        inventory = new();
        equipedItems = new();
        defeatedBosses = new();

        equipedWeapon[0] = WeaponType.Sword;
        equipedWeapon[1] = WeaponType.Gun;
    }

    public int GetPercentageComplete()
    {
        //TODO: use save points to calculate percentage complete
        return 24;
    }
}
