using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

    public PlayerMovementSkills unlockedMovementSkills;
    public PlayerTimeSkills unlockedTimeSkills;

    public SerializableDictionary<string, bool> defeatedBosses;
    public SerializableDictionary<string, bool> pickedTreasures;
    public SerializableDictionary<string, SavepointDetails> savepoints;

    public SerializableDictionary<string, ItemData> statusEnhancementInventory;
    public SerializableDictionary<string, ItemData> consumablesInventory;
    public SerializableDictionary<string, ItemData> chipInventory;

    public SerializableDictionary<string, ItemData> storyItemInventory;
    public SerializableDictionary<string, ItemData> movementSkillItemInventory;
    public SerializableDictionary<string, ItemData> timeSkillItemInventory;

    public SerializableDictionary<string, string> equipedItems;
    public WeaponType[] equipedWeapon = new WeaponType[2];

    public GameData()
    {
        maxHealth = 100f;
        timePlayed = 0f;
        money = 0;
        currentTimeSkill = "PlayerTimeSkill_None";
        lastInteractedSavepoint = "Defult";

        unlockedMovementSkills = new();
        unlockedTimeSkills = new();

        storyItemInventory = new();
        movementSkillItemInventory = new();
        timeSkillItemInventory = new();

        statusEnhancementInventory = new();
        savepoints = new();
        chipInventory = new();
        pickedTreasures = new();
        equipedItems = new();
        defeatedBosses = new();
        consumablesInventory = new();
        consumablesInventory.Add("Medkit", new ItemData(3, "Medkit"));

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

[System.Serializable]
public class PlayerTimeSkills
{
    public bool timeStopRanged = false;
    public bool timeStopAll = false;
    public bool timeSlowRanged = false;
    public bool timeSlowAll = false;
    public bool timeReverse = false;
    public bool bookMark = false;

}

[System.Serializable]
public class PlayerMovementSkills
{
    public SerializableDictionary<string, bool> unlockedMovementSkills = new()
    {
        {"DoubleJump", false },
        {"Dash", false },
        {"WallJump", false }
    };
}
