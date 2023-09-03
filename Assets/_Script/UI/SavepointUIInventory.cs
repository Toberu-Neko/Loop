using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavepointUIInventory : MonoBehaviour
{
    [SerializeField] private SavepointUIMain savepointUIMain;

    [SerializeField] private GameObject draggablePrefab;
    private List<GameObject> draggableItems;

    private LootSO[] lootSOs;
    private Dictionary<string, Sprite> lootSODict;

    [SerializeField] private GameObject inventoryGrid;
    private InventorySlot[] inventorySlots;
    [SerializeField] private GameObject swordGrid;
    private EquipmentSlot[] swordSlots;
    [SerializeField] private GameObject gunGrid;
    private EquipmentSlot[] gunSlots;
    [SerializeField] private GameObject fistGrid;
    private EquipmentSlot[] fistSlots;

    private SerializableDictionary<string, ItemData> inventory;

    private void Awake()
    {
        var lootSOs = Resources.LoadAll<LootSO>("LootSO");
        this.lootSOs = lootSOs;
        lootSODict = new();
        foreach (var item in lootSOs)
        {
            lootSODict.Add(item.lootDetails.lootName, item.lootSprite);
        }

        inventorySlots = inventoryGrid.transform.GetComponentsInChildren<InventorySlot>();

        var slots = swordGrid.transform.GetComponentsInChildren<Transform>();
        swordSlots = new EquipmentSlot[slots.Length - 1];
        for (int i = 1; i < slots.Length; i++)
        {
            swordSlots[i - 1] = slots[i].gameObject.GetComponent<EquipmentSlot>();
        }

        slots = gunGrid.transform.GetComponentsInChildren<Transform>();
        gunSlots = new EquipmentSlot[slots.Length - 1];
        for (int i = 1; i < slots.Length; i++)
        {
            gunSlots[i - 1] = slots[i].gameObject.GetComponent<EquipmentSlot>();
        }

        slots = fistGrid.transform.GetComponentsInChildren<Transform>();
        fistSlots = new EquipmentSlot[slots.Length - 1];
        for (int i = 1; i < slots.Length; i++)
        {
            fistSlots[i - 1] = slots[i].gameObject.GetComponent<EquipmentSlot>();
        }

        draggableItems = new();
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

        for(int i=0;i<inventorySlots.Length;i++)
        {
            inventorySlots[i].DeactiveSlot();
        }
    }

    private void UpdateInventory()
    {
        inventory = PlayerInventoryManager.Instance.Inventory;
        int count = 0;
        foreach(var item in inventory)
        {
            inventorySlots[count].ActiveSlot();
            inventorySlots[count].SetValue(item.Value.itemCount, lootSODict[item.Value.lootDetails.lootName]);

            count++;
        }

        for (int i = count; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].DeactiveSlot();
        }
    }
}
