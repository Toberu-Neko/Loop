using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler, IDataPersistance
{
    private SavepointUIInventory savepointUIInventory;
    private InventorySlot inventorySlot;
    [SerializeField] private GameObject clickAndReturnPrefab;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField, Range(0, 3)] private int slotIndex;

    public LootSO LootSO { get; private set; }
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

        if (draggableItem == null)
        {
            return;
        }

        if ((equipmentType == EquipmentType.Sword && draggableItem.CanEquipOnSword)
            ||
            (equipmentType == EquipmentType.Gun && draggableItem.CanEquipOnGun)
            ||
            (equipmentType == EquipmentType.Fist && draggableItem.CanEquipOnFist))
        {
            draggableItem.DontHaveTarget = false;
            draggableItem.ParentAfterDrag = transform;
            LootSO = draggableItem.LootSO;


            clickAndReturnObj = ObjectPoolManager.SpawnObject(clickAndReturnPrefab, transform);
            clickAndReturn = clickAndReturnObj.GetComponent<ClickAndReturn>();
            clickAndReturn.SetValue(LootSO);
            clickAndReturn.OnReturn += HandleReturn;

            clickAndReturnObj.transform.localPosition = Vector3.zero;
        }
    }

    public void SaveData(GameData data)
    {
        if(data.equipedItems.ContainsKey(equipmentType.ToString() + slotIndex))
        {
            data.equipedItems.Remove(equipmentType.ToString() + slotIndex);
        }

        if(LootSO != null)
            data.equipedItems.Add(equipmentType.ToString() + slotIndex, LootSO.lootDetails.lootName);
        else
            data.equipedItems.Remove(equipmentType.ToString() + slotIndex);
    }

    public void LoadData(GameData data)
    {
        if(data.equipedItems.ContainsKey(equipmentType.ToString() + slotIndex))
        {
            string lootName = data.equipedItems[equipmentType.ToString() + slotIndex];
            LootSO so = ItemDataManager.Instance.LootSODict[lootName];

            if(so != null)
            {
                GameObject obj = ObjectPoolManager.SpawnObject(clickAndReturnPrefab, transform);
                obj.transform.localPosition = Vector3.zero;
                ClickAndReturn script = obj.GetComponent<ClickAndReturn>();
                script.SetValue(so);
                script.OnReturn += HandleReturn;
                clickAndReturnObj = obj;
                clickAndReturn = script;
                LootSO = so;
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

        clickAndReturnObj = null;
        clickAndReturn = null;
        LootSO = null;
    }
}
public enum EquipmentType
{
    Sword,
    Gun,
    Fist
}
