using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class MapItem_Burner : InteractableMapItem_Base, IDataPersistance
{
    [SerializeField] private string itemName = "burner";
    [SerializeField] private SO_ConsumeableItem targetItem;
    [SerializeField] private int maxNeededCount = 3;
    [SerializeField] private Collider2D col;

    [SerializeField] private GameObject teleportObj;
    [SerializeField] private Animator doorAnim;
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
            if(doorAnim!=null)
                doorAnim.SetBool("AlwaysOpen", true);
            teleportObj.SetActive(true);
            interactable = false;
            col.enabled = false;
        }
        else
        {
            interactable = true;
            teleportObj.SetActive(false);
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
            //Play Open animation
            descriptionStringEvent.StringReference = openedText;

            if (doorAnim != null)
                doorAnim.SetBool("Open", true);
            teleportObj.SetActive(true);
            col.enabled = false;
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
        if (inv.ConsumablesInventory.ContainsKey(targetItem.ID) && interactable)
        {
            if (inv.ConsumablesInventory[targetItem.ID].itemCount > 0)
            {
                AudioManager.instance.PlaySoundFX(interactSFX, transform, AudioManager.SoundType.twoD);
                inv.RemoveConsumableItem(targetItem.ID);
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
