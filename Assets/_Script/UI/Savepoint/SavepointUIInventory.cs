using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class SavepointUIInventory : MonoBehaviour
{
    [SerializeField] private SavepointUIMain savepointUIMain;

    [SerializeField] private GameObject inventoryGrid;

    [SerializeField] private GameObject descriptionUI;
    [SerializeField] private LocalizeStringEvent itemNameStringEvent;
    [SerializeField] private LocalizeStringEvent descriptionStringEvent;
    [SerializeField] private LocalizedString defaultString;

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
        descriptionUI.SetActive(false);

        InventorySlots = inventoryGrid.transform.GetComponentsInChildren<InventorySlot>();

        swordSlots = swordGrid.transform.GetComponentsInChildren<EquipmentSlot>();

        gunSlots = gunGrid.transform.GetComponentsInChildren<EquipmentSlot>();

        fistSlots = fistGrid.transform.GetComponentsInChildren<EquipmentSlot>();
    }

    public void ActiveDescriptionUI(LocalizedString name, LocalizedString description)
    {
        descriptionUI.SetActive(true);
        itemNameStringEvent.StringReference = name;
        descriptionStringEvent.StringReference = description;
    }

    public void DeactiveDescriptionUI()
    {
        descriptionUI.SetActive(false);
        itemNameStringEvent.StringReference = defaultString;
        descriptionStringEvent.StringReference = defaultString;
    }

    public void OnClickBackButton()
    {
        Deactivate();
        savepointUIMain.ActivateMenu();
    }

    public void ActiveMenu()
    {
        gameObject.SetActive(true);
        UpdateInventory();
    }

    public void Deactivate()
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
        inventory = PlayerInventoryManager.Instance.ChipInventory;
        int count = 0;

        foreach(var item in inventory)
        {
            SO_Chip so = ItemDataManager.Instance.ChipDict[item.Value.itemdataID];
            if (so == null) 
            {
                Debug.LogError("ItemDataManager.Instance.ChipDict[item.Value.itemName] == null");
                continue;
            }
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
            InventorySlots[count].OnEnterTarget += HandleEnterTarget;
            InventorySlots[count].OnExitTarget += HandleExitTarget;

            count++;
        }

        for (int i = count; i < InventorySlots.Length; i++)
        {
            InventorySlots[i].DeactiveSlot();
        }
    }

    private void HandleEnterTarget(LocalizedString name, LocalizedString description)
    {
        ActiveDescriptionUI(name, description);
    }

    private void HandleExitTarget()
    {
        DeactiveDescriptionUI();
    }
}
