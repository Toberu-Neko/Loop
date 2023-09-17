using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTreasure : PressEPickItemBase, IDataPersistance
{
    public bool isAddedID;
    public string ID;

    [SerializeField] private SO_Treasure so;

    private bool ispicked;

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
        switch(so.treasureType)
        {
            case SO_Treasure.TreasureType.Chip:
                // PlayerInventoryManager.Instance.AddChip(so.addMaxHealth);
                break;
            case SO_Treasure.TreasureType.StoryItem:
                // PlayerInventoryManager.Instance.AddItem(so.addMaxHealth);
                break;
            case SO_Treasure.TreasureType.Movement:
                // PlayerInventoryManager.Instance.AddMovementSkill(so.playerMovementSkills);
                break;
            case SO_Treasure.TreasureType.TimeSkill:
                // PlayerInventoryManager.Instance.AddTimeSkill(so.timeSkills);
                break;
            case SO_Treasure.TreasureType.PlayerStatusEnhancement:
                // PlayerInventoryManager.Instance.AddPlayerStatusEnhancement(so.addMaxHealth);
                break;
        }

        /*
        UI_Manager.Instance.ActivePickupItemUI(lootSO.itemDetails.lootName, lootSO.itemDescription);
        PlayerInventoryManager.Instance.AddChip(lootSO.itemDetails);
        */
        ispicked = true;
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.SaveGame();
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
