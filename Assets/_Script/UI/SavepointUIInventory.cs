using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavepointUIInventory : MonoBehaviour
{
    [SerializeField] private SavepointUIMain savepointUIMain;

    [SerializeField] private GameObject draggablePrefab;

    [SerializeField] private GameObject inventoryGrid;

    public InventorySlot[] InventorySlots { get; private set;}

    [SerializeField] private GameObject swordGrid;
    private EquipmentSlot[] swordSlots;
    [SerializeField] private GameObject gunGrid;
    private EquipmentSlot[] gunSlots;
    [SerializeField] private GameObject fistGrid;
    private EquipmentSlot[] fistSlots;

    private SerializableDictionary<string, ItemData> inventory;

    private void Awake()
    {
        InventorySlots = inventoryGrid.transform.GetComponentsInChildren<InventorySlot>();

        swordSlots = swordGrid.transform.GetComponentsInChildren<EquipmentSlot>();

        gunSlots = gunGrid.transform.GetComponentsInChildren<EquipmentSlot>();

        fistSlots = fistGrid.transform.GetComponentsInChildren<EquipmentSlot>();
    }

    public void OnClickBackButton()
    {
        DeactiveMenu();
        savepointUIMain.ActiveMenu();
    }

    public void ActiveMenu()
    {
        gameObject.SetActive(true);
        UpdateInventory();
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);

        if(InventorySlots != null)
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                InventorySlots[i].DeactiveSlot();
            }
        }

        DataPersistenceManager.Instance.SaveGame();
    }

    private void UpdateInventory()
    {
        inventory = PlayerInventoryManager.Instance.Inventory;
        int count = 0;

        foreach(var item in inventory)
        {
            LootSO so = ItemDataManager.Instance.LootSODict[item.Value.lootDetails.lootName];
            int SOcount = 0;

            foreach(EquipmentSlot equipmentSlot in swordSlots)
            {
                if(equipmentSlot.LootSO == so)
                {
                    SOcount++;
                }
            }

            foreach (EquipmentSlot equipmentSlot in gunSlots)
            {
                if (equipmentSlot.LootSO == so)
                {
                    SOcount++;
                }
            }

            foreach (EquipmentSlot equipmentSlot in fistSlots)
            {
                if (equipmentSlot.LootSO == so)
                {
                    SOcount++;
                }
            }


            InventorySlots[count].ActiveSlot();
            InventorySlots[count].SetValue(item.Value.itemCount - SOcount, so);

            count++;
        }

        for (int i = count; i < InventorySlots.Length; i++)
        {
            InventorySlots[i].DeactiveSlot();
        }
    }
}
