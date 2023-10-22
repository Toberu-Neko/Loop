using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupTreasure : PressEPickItemBase, IDataPersistance
{
    [Header("ID")]
    public bool isAddedID;
    public string ID;

    [Header("Treasure Data")]
    [SerializeField] private TreasureType treasureType;

    [Header("Chip")]
    [SerializeField] private SO_Chip chip;

    [Header("StoryItem")]
    [SerializeField] private SO_StoryItem storyItem;

    [Header("PlayerStatusEnhancement")]
    [SerializeField] private SO_PlayerStatusEnhancement playerStatusEnhancement;

    [Header("Skill")]
    [SerializeField] private SO_TimeSkillItem timeSkills;

    [Header("Movement")]
    [SerializeField] private SO_MovementSkillItem movementSkills;

    [Header("Consumable")]
    [SerializeField] private SO_ConsumeableItem consumeableItem;

    [Header("Weapon")]
    [SerializeField] private SO_WeaponItem weaponItem;

    public enum TreasureType
    {
        Chip,
        PlayerStatusEnhancement,
        StoryItem,
        Movement,
        TimeSkill,
        Weapon,
        Consumable
    }

    private bool ispicked;
    private PlayerWeaponManager playerWeaponManager;

    protected override void Start()
    {
        base.Start();

        DataPersistenceManager.Instance.GameData.pickedTreasures.TryGetValue(ID, out ispicked);

        if (ispicked)
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnItemPicked += HandlePickUp;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnItemPicked -= HandlePickUp;
    }

    private void HandlePickUp()
    {
        switch(treasureType)
        {
            case TreasureType.Chip:
                PlayerInventoryManager.Instance.AddChip(chip.itemName);
                UI_Manager.Instance.ActivePickupItemUI(chip.displayName, chip.itemDescription);
                break;
            case TreasureType.StoryItem:
                PlayerInventoryManager.Instance.AddStoryItem(storyItem.itemName);
                UI_Manager.Instance.ActivePickupItemUI(storyItem.displayName, storyItem.itemDescription);
                break;
            case TreasureType.Movement:
                PlayerInventoryManager.Instance.AddMovemnetSkillItem(movementSkills.itemName);
                UI_Manager.Instance.ActivePickupItemUI(movementSkills.displayName, movementSkills.itemDescription);
                break;
            case TreasureType.TimeSkill:
                PlayerInventoryManager.Instance.AddTimeSkillItem(timeSkills.itemName);
                UI_Manager.Instance.ActivePickupItemUI(timeSkills.displayName, timeSkills.itemDescription);
                break;
            case TreasureType.PlayerStatusEnhancement:
                PlayerInventoryManager.Instance.AddPlayerStatusEnhancementItem(playerStatusEnhancement.itemName);
                UI_Manager.Instance.ActivePickupItemUI(playerStatusEnhancement.displayName, playerStatusEnhancement.itemDescription);
                break;
            case TreasureType.Consumable:
                PlayerInventoryManager.Instance.AddConsumableItem(consumeableItem.itemName);
                UI_Manager.Instance.ActivePickupItemUI(consumeableItem.displayName, consumeableItem.itemDescription);
                break;
            case TreasureType.Weapon:
                PlayerInventoryManager.Instance.AddWeaponItem(weaponItem.itemName);
                UI_Manager.Instance.ActivePickupItemUI(weaponItem.displayName, weaponItem.itemDescription);

                if(PlayerInventoryManager.Instance.CanUseWeaponCount == 0)
                {
                    if (weaponItem.unlockGun)
                    {
                        playerWeaponManager.EquipWeapon(WeaponType.Gun);
                        PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Gun);
                    }
                    if(weaponItem.unlockSword)
                    {
                        playerWeaponManager.EquipWeapon(WeaponType.Sword);
                        PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Sword);
                    }
                    if(weaponItem.unlockFist)
                    {
                        playerWeaponManager.EquipWeapon(WeaponType.Fist);
                        PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Fist);
                    }
                }

                if(PlayerInventoryManager.Instance.CanUseWeaponCount == 1)
                {
                    if (weaponItem.unlockGun)
                    {
                        PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Gun);
                    }
                    if (weaponItem.unlockSword)
                    {
                        PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Sword);
                    }
                    if (weaponItem.unlockFist)
                    {
                        PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Fist);
                    }
                }
                break;
        }

        ispicked = true;
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.SaveGame();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(treasureType == TreasureType.Weapon)
        {
            if (collision.CompareTag("Player"))
            {
                if (playerWeaponManager == null)
                {
                    playerWeaponManager = collision.GetComponent<PlayerWeaponManager>();
                }
            }
        }
    }

    public void LoadData(GameData data)
    {
        Debug.LogError("This should not be called");
    }

    public void SaveData(GameData data)
    {
        if(data.pickedTreasures.ContainsKey(ID))
        {
            data.pickedTreasures[ID] = ispicked;
        }
        else
        {
            data.pickedTreasures.Add(ID, ispicked);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PickupTreasure))]
public class PickupTreasureEditor : Editor
{
    PickupTreasure treasure;

    private void OnEnable()
    {
        treasure = (PickupTreasure)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*
        so.treasureType = (SO_Treasure.TreasureType) EditorGUILayout.EnumPopup("Treasure Type", so.treasureType);

        if(so.treasureType == SO_Treasure.TreasureType.Chip)
        {
            EditorGUILayout.LabelField("Chip", EditorStyles.boldLabel);
            so.chip = (SO_Chip)EditorGUILayout.ObjectField("Chip", so.chip, typeof(SO_Chip), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.StoryItem)
        {
            EditorGUILayout.LabelField("Story Item", EditorStyles.boldLabel);
            so.storyItem = (SO_StoryItem)EditorGUILayout.ObjectField("Story Item", so.storyItem, typeof(SO_StoryItem), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.PlayerStatusEnhancement)
        {
            EditorGUILayout.LabelField("Player Status Enhancement", EditorStyles.boldLabel);
            so.playerStatusEnhancement = (SO_PlayerStatusEnhancement)EditorGUILayout.ObjectField("Player Status Enhancement", so.playerStatusEnhancement, typeof(SO_PlayerStatusEnhancement), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.TimeSkill)
        {
            EditorGUILayout.LabelField("Time Skill", EditorStyles.boldLabel);
            so.timeSkills = (SO_TimeSkillItem)EditorGUILayout.ObjectField("Time Skill", so.timeSkills, typeof(SO_TimeSkillItem), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.Movement)
        {
            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
            so.movementSkills = (SO_MovementSkillItem)EditorGUILayout.ObjectField("Movement", so.movementSkills, typeof(SO_MovementSkillItem), true);
        }
        */

        if (GUI.changed)
            EditorUtility.SetDirty(treasure);
    }
}
#endif
