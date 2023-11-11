using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseInventoryMain : MonoBehaviour
{
    [SerializeField] private PauseUIMain pauseUIMain;
    [SerializeField] private PauseInvDescription pauseInvDescription;

    [SerializeField] private GameObject inventoryGrid;
    private PauseInventorySlot[] inventorySlots;
    private List<PauseInventorySlot> activeInventorySlots;

    private void Awake()
    {
        inventorySlots = inventoryGrid.GetComponentsInChildren<PauseInventorySlot>();
    }

    private void OnEnable()
    {
        activeInventorySlots = new();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        pauseInvDescription.Deactivate();
        OnClickStoryButtion();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        pauseInvDescription.Deactivate();
    }

    public void OnClickBackButton()
    {
        Deactivate();
        pauseUIMain.ActivateMenu();
    }

    private void HandleOnExit()
    {
        pauseInvDescription.Deactivate();
    }

    private void HandleOnEnter(SO_ItemsBase obj)
    {
        pauseInvDescription.Activate(obj);
    }
    private void ResetSlots()
    {
        foreach(var slot in activeInventorySlots)
        {
            slot.OnEnter -= HandleOnEnter;
            slot.OnExit -= HandleOnExit;
        }

        activeInventorySlots.Clear();
    }


    private void UpdateSlot(SerializableDictionary<string, ItemData> data, ItemType itemType)
    {
        int count = 0;

        foreach (var item in data)
        {
            SO_ItemsBase so = null;
            switch(itemType)
            {
                case ItemType.Chip:
                    so = ItemDataManager.Instance.ChipDict[item.Value.itemName];
                    break;
                case ItemType.MovementSkill:
                    so = ItemDataManager.Instance.MovementSkillDict[item.Value.itemName];
                    break;
                case ItemType.TimeSkill:
                    so = ItemDataManager.Instance.TimeSkillDict[item.Value.itemName];
                    break;
                case ItemType.StatusEnhancement:
                    so = ItemDataManager.Instance.StatusEnhancementDict[item.Value.itemName];
                    break;
                case ItemType.StoryItem:
                    so = ItemDataManager.Instance.StoryItemDict[item.Value.itemName];
                    break;
                case ItemType.ConsumableItem:
                    so = ItemDataManager.Instance.ConsumableItemDict[item.Value.itemName];
                    break;
                case ItemType.WeaponItem:
                    so = ItemDataManager.Instance.WeaponItemDict[item.Value.itemName];
                    break;
            }

            if (so == null)
            {
                Debug.LogError("ItemDataManager.Instance.StoryItemDict[item.Value.itemName] == null");
            }

            activeInventorySlots.Add(inventorySlots[count]);
            inventorySlots[count].SetItem(so, item.Value.itemCount);
            inventorySlots[count].OnEnter += HandleOnEnter;
            inventorySlots[count].OnExit += HandleOnExit;

            count++;
        }

        for (int i = count; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].Deactivate();
        }
    }

    public void OnClickStoryButtion()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.StoryItemInventory, ItemType.StoryItem);
    }

    public void OnClickChipButtion()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.ChipInventory, ItemType.Chip);
    }

    public void OnClickConsumableButton()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.ConsumablesInventory, ItemType.ConsumableItem);
    }

    public void OnClickWeaponButton()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.WeaponInventory, ItemType.WeaponItem);
    }

    public void OnClickStatusEnhancementButton()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.StatusEnhancementInventory, ItemType.StatusEnhancement);
    }

    public void OnClickMovementSkillButton()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.MovementSkillItemInventory, ItemType.MovementSkill);
    }

    public void OnClickTimeSkillButton()
    {
        ResetSlots();
        UpdateSlot(PlayerInventoryManager.Instance.TimeSkillItemInventory, ItemType.TimeSkill);
    }
}
