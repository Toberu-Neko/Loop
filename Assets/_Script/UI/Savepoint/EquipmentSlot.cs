using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler, IDataPersistance
{
    private SavepointUIInventory savepointUIInventory;
    private InventorySlot inventorySlot;
    [SerializeField] private GameObject clickAndReturnPrefab;
    [SerializeField] private WeaponType equipmentType;
    [SerializeField, Range(0, 3)] private int slotIndex;

    public SO_Chip LootSO { get; private set; }
    private GameObject clickAndReturnObj;
    private ClickAndReturn clickAndReturn;
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            return;
        }

        GameObject droppedItem = eventData.pointerDrag;
        droppedItem.TryGetComponent(out DraggableItem draggableItem);

        if (draggableItem == null || draggableItem.Count <= 0)
        {
            return;
        }

        if ((equipmentType == WeaponType.Sword && draggableItem.CanEquipOnSword)
            ||
            (equipmentType == WeaponType.Gun && draggableItem.CanEquipOnGun)
            ||
            (equipmentType == WeaponType.Fist && draggableItem.CanEquipOnFist))
        {
            draggableItem.DontHaveTarget = false;
            draggableItem.ParentAfterDrag = transform;

            UpdateSlot(draggableItem.LootSO);
        }
    }

    private void UpdateSlot(SO_Chip SO)
    {
        LootSO = SO;
        clickAndReturnObj = ObjectPoolManager.SpawnObject(clickAndReturnPrefab, transform);
        clickAndReturn = clickAndReturnObj.GetComponent<ClickAndReturn>();
        clickAndReturn.SetValue(SO);
        clickAndReturn.OnReturn += HandleReturn;

        clickAndReturnObj.transform.localPosition = Vector3.zero;

        PlayerInventoryManager.Instance.EquipChip(SO, equipmentType);
    }

    public void SaveData(GameData data)
    {
        // Debug.Log("SaveSlot" + data.equipedItems.Count);
        if(data.equipedItems.ContainsKey(equipmentType.ToString() + slotIndex))
        {
            data.equipedItems.Remove(equipmentType.ToString() + slotIndex);
        }

        if(LootSO != null)
            data.equipedItems.Add(equipmentType.ToString() + slotIndex, LootSO.itemName);
        else
            data.equipedItems.Remove(equipmentType.ToString() + slotIndex);
    }

    public void LoadData(GameData data)
    {
        // Debug.Log("LoadSlot: " + data.equipedItems.Count);
        if(data.equipedItems.ContainsKey(equipmentType.ToString() + slotIndex))
        {
            string lootName = data.equipedItems[equipmentType.ToString() + slotIndex];
            SO_Chip so = ItemDataManager.Instance.ChipDict[lootName];

            if(so != null)
            {
                UpdateSlot(so);
            }
        }
    }

    private void HandleReturn()
    {
        savepointUIInventory = GetComponentInParent<SavepointUIInventory>();
        var inventorySlots = savepointUIInventory.InventorySlots;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].LootSO == LootSO)
            {
                inventorySlot = inventorySlots[i];
                break;
            }
        }

        clickAndReturn.OnReturn -= HandleReturn;
        clickAndReturnObj.transform.SetParent(transform.root);
        inventorySlot.SetCount(inventorySlot.Count + 1);
        ObjectPoolManager.ReturnObjectToPool(clickAndReturnObj);
        PlayerInventoryManager.Instance.UnEquipChip(LootSO, equipmentType);

        clickAndReturnObj = null;
        clickAndReturn = null;
        LootSO = null;
    }
}
