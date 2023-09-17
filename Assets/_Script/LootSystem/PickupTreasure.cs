using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTreasure : PressEPickItemBase, IDataPersistance, IUniqueID
{
    public bool isAddedID { get; set; }
    public string ID { get; set; }

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
        /*
        UI_Manager.Instance.ActivePickupItemUI(lootSO.itemDetails.lootName, lootSO.itemDescription);
        PlayerInventoryManager.Instance.AddChip(lootSO.itemDetails);
        */
        DataPersistenceManager.Instance.SaveGame();
        ispicked = true;
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
