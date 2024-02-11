using UnityEngine;
using UnityEngine.Localization.Components;


public class PickupTreasure : PressEPickItemBase, IDataPersistance
{
    [Header("ID")]
    public bool isAddedID;
    public string ID;

    [Header("Treasure Data")]
    [SerializeField] private SO_ItemsBase treasureData;

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
    
    private void EquipWeapon(SO_WeaponItem item)
    {
        if(item.unlockFist && item.unlockGun && item.unlockSword)
        {
            playerWeaponManager.EquipWeapon(WeaponType.Sword);
            PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Sword);
            PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Gun);
            return;
        }

        if (PlayerInventoryManager.Instance.CanUseWeaponCount == 1)
        {
            if (item.unlockGun)
            {
                playerWeaponManager.EquipWeapon(WeaponType.Gun);
                PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Gun);
            }
            if (item.unlockSword)
            {
                playerWeaponManager.EquipWeapon(WeaponType.Sword);
                PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Sword);
            }
            if (item.unlockFist)
            {
                playerWeaponManager.EquipWeapon(WeaponType.Fist);
                PlayerInventoryManager.Instance.ChangeEquipWeapon1(WeaponType.Fist);
            }
        }

        if (PlayerInventoryManager.Instance.CanUseWeaponCount == 2)
        {
            if (item.unlockGun)
            {
                PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Gun);
            }
            if (item.unlockSword)
            {
                PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Sword);
            }
            if (item.unlockFist)
            {
                PlayerInventoryManager.Instance.ChangeEquipWeapon2(WeaponType.Fist);
            }
        }
    }

    private void HandlePickUp()
    {
        switch (treasureData)
        {
            case SO_Chip:
                PlayerInventoryManager.Instance.AddChip(treasureData.itemName);
                break;
            case SO_ConsumeableItem:
                PlayerInventoryManager.Instance.AddConsumableItem(treasureData.itemName);
                break;
            case SO_PlayerStatusEnhancement:
                PlayerInventoryManager.Instance.AddPlayerStatusEnhancementItem(treasureData.itemName);
                break;
            case SO_StoryItem:
                PlayerInventoryManager.Instance.AddStoryItem(treasureData.itemName);
                break;
            case SO_TimeSkillItem:
                PlayerInventoryManager.Instance.AddTimeSkillItem(treasureData.itemName);
                break;
            case SO_MovementSkillItem:
                PlayerInventoryManager.Instance.AddMovemnetSkillItem(treasureData.itemName);
                break;
            case SO_WeaponItem:
                PlayerInventoryManager.Instance.AddWeaponItem(treasureData.itemName);
                SO_WeaponItem item = (SO_WeaponItem)treasureData;
                EquipWeapon(item);
                break;
        }

        UI_Manager.Instance.ActivePickupItemUI(treasureData.displayNameLocalization, treasureData.shortDescriptionLocalization);

        ispicked = true;
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.SaveGame();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(treasureData is SO_WeaponItem)
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
