using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int money;
    public float maxHealth;
    public float timePlayed;
    public Vector2 playerPosition;
    public bool gotoSavePoint;
    public bool firstTimePlaying;

    public string currentTimeSkill;
    public string lastInteractedSavepointID;

    public bool finishTutorial;

    public PlayerMovementSkills unlockedMovementSkills;
    public PlayerTimeSkills unlockedTimeSkills;

    public SerializableDictionary<string, int> interactableMapItem;

    public SerializableDictionary<string, bool> defeatedBosses;
    public SerializableDictionary<string, bool> pickedTreasures;
    public SerializableDictionary<string, bool> activatedMapItem;

    public SerializableDictionary<string, SavepointDetails> savepoints;
    public SerializableDictionary<string, ShopInventory> shopsData;

    public SerializableDictionary<string, ItemData> statusEnhancementInventory;
    public SerializableDictionary<string, ItemData> consumablesInventory;
    public SerializableDictionary<string, ItemData> chipInventory;

    public SerializableDictionary<string, ItemData> storyItemInventory;
    public SerializableDictionary<string, ItemData> movementSkillItemInventory;
    public SerializableDictionary<string, ItemData> timeSkillItemInventory;
    public SerializableDictionary<string, ItemData> weaponInventory;

    public SerializableDictionary<string, string> equipedItems;
    public WeaponType[] equipedWeapon = new WeaponType[2];

    public GameData()
    {
        firstTimePlaying = true;

        maxHealth = 100f;
        timePlayed = 0f;
        money = 0;
        currentTimeSkill = "PlayerTimeSkill_None";
        lastInteractedSavepointID = "Default";
        finishTutorial = false;
        gotoSavePoint = true;
        playerPosition = new Vector2(0, 0);

        interactableMapItem = new();
        unlockedMovementSkills = new();
        unlockedTimeSkills = new();

        storyItemInventory = new();
        movementSkillItemInventory = new();
        timeSkillItemInventory = new();
        weaponInventory = new();

        shopsData = new();

        statusEnhancementInventory = new();
        savepoints = new();
        chipInventory = new();
        pickedTreasures = new();
        equipedItems = new();
        defeatedBosses = new();
        consumablesInventory = new()
        {
            { "Medkit", new ItemData(0, "Medkit") }
        };

        equipedWeapon[0] = WeaponType.None;
        equipedWeapon[1] = WeaponType.None;
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
public class ShopInventory
{
    public SerializableDictionary<string, ItemData> shopInventory;

    public ShopInventory()
    {
        shopInventory = new();
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
    public bool doubleJump = false;
    public bool dash = false;
    public bool wallJump = false;
}
