using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int money;
    public float maxHealth;
    public float timePlayed;

    public string currentTimeSkill;
    public string lastInteractedSavepoint;

    public SerializableDictionary<string, bool> defeatedBosses;
    public SerializableDictionary<string, bool> pickedTreasures;
    public SerializableDictionary<string, SavepointDetails> savepoints;


    public SerializableDictionary<string, ItemData> inventory;
    public SerializableDictionary<string, string> equipedItems;
    public WeaponType[] equipedWeapon = new WeaponType[2];

    public GameData()
    {
        maxHealth = 100f;
        timePlayed = 0f;
        money = 0;
        currentTimeSkill = "PlayerTimeSkill_None";
        lastInteractedSavepoint = "Defult";

        savepoints = new();
        inventory = new();
        pickedTreasures = new();
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

[System.Serializable]
public class SavepointDetails
{
    public bool isActivated = false;
    public Vector3 teleportPosition;

    public SavepointDetails(bool isSavePointActive, Vector3 teleportPos)
    {
        isActivated = isSavePointActive;
        teleportPosition = teleportPos;
    }
}
