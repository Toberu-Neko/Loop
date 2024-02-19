using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class MapItem_Burner : InteractableMapItem_Base, IDataPersistance
{
    [SerializeField] private string itemName = "burner";
    [SerializeField] private string counsumableName = "shan";
    [SerializeField] private int maxNeededCount = 3;

    [SerializeField] private GameObject door;
    [SerializeField] private GameObject textObj;
    [SerializeField] private LocalizeStringEvent descriptionStringEvent;

    [SerializeField] private LocalizedString noItemText;
    [SerializeField] private LocalizedString gaveItemText;
    [SerializeField] private LocalizedString openedText;
    [SerializeField] private Sound interactSFX;
    private int onItemConsumableCount;

    private event Action OnItemConsumableCountChange;

    protected override void Start()
    {
        base.Start();

        if(DataPersistenceManager.Instance.GameData.interactableMapItem.ContainsKey(itemName))
            onItemConsumableCount = DataPersistenceManager.Instance.GameData.interactableMapItem[itemName];
        else
            onItemConsumableCount = 0;

        if(onItemConsumableCount >= maxNeededCount)
        {
            // Debug.Log("Burner is on");
            door.SetActive(false);
            interactable = false;
        }
        else
        {
            interactable = true;
        }

        textObj.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnInteract += HandleInteract;
        OnItemConsumableCountChange += OnTargetItemChange;
    }

    private void OnTargetItemChange()
    {
        if(onItemConsumableCount >= maxNeededCount)
        {
            //TODO: Play animation
            descriptionStringEvent.StringReference = openedText;
            door.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnInteract -= HandleInteract;
        OnItemConsumableCountChange -= OnTargetItemChange;
    }

    private void HandleInteract()
    {
        PlayerInventoryManager inv = PlayerInventoryManager.Instance;
        if (inv.ConsumablesInventory.ContainsKey(counsumableName))
        {
            if (inv.ConsumablesInventory[counsumableName].itemCount > 0)
            {
                AudioManager.instance.PlaySoundFX(interactSFX, transform);
                inv.RemoveConsumableItem(counsumableName);
                onItemConsumableCount++;
                OnItemConsumableCountChange?.Invoke();
                descriptionStringEvent.StringReference = gaveItemText;
                DataPersistenceManager.Instance.SaveGame();
            }
            else
            {
                descriptionStringEvent.StringReference = noItemText;
            }
        }
        else
        {
            descriptionStringEvent.StringReference = noItemText;
        }

        TextObjOn();
    }

    private void TextObjOn()
    {
        CancelInvoke(nameof(TextObjOff));
        textObj.SetActive(true);
        Invoke(nameof(TextObjOff), 2f);
    }

    private void TextObjOff()
    {
        CancelInvoke(nameof(TextObjOff));
        textObj.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        Debug.LogError("This should not be called");
    }

    public void SaveData(GameData data)
    {
        if(data.interactableMapItem.ContainsKey(itemName))
        {
            data.interactableMapItem[itemName] = onItemConsumableCount;
        }
        else
        {
            data.interactableMapItem.Add(itemName, onItemConsumableCount);
        }
    }
}
