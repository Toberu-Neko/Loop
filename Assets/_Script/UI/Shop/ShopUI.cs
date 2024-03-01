using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ShopUI : MonoBehaviour, IDataPersistance
{
    [SerializeField] private LocalizeStringEvent shopName;
    [SerializeField] private Transform iniventoryGrid;

    [SerializeField] private GameObject descriptionObj;
    [SerializeField] private LocalizeStringEvent descriptionName;
    [SerializeField] private LocalizeStringEvent descriptionText;
    [SerializeField] private TextMeshProUGUI descriptionPriceText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private GameObject firstSelectedObj;

    private SerializableDictionary<string, ShopInventory> shopsData;
    private SerializableDictionary<string, ItemData> inventory;

    private ShopSlot[] shopSlots;

    private void Awake()
    {
        shopSlots = iniventoryGrid.GetComponentsInChildren<ShopSlot>();

        foreach (var slot in shopSlots)
        {
            slot.OnEnterTarget += OnPointerEnter;
            slot.OnClick += OnShopSlotClick;
            slot.OnExitTarget += OnPointerExit;
        }
    }

    private void OnEnable()
    {
        inventory = new();
    }


    private void OnShopSlotClick(SO_ItemsBase itemBase)
    {
        if (!inventory.ContainsKey(itemBase.ID))
        {
            Debug.LogError("Item not found in inventory");
            return;
        }

        ItemData shopItem = inventory[itemBase.ID];

        if (PlayerInventoryManager.Instance.Money >= itemBase.price && shopItem.itemCount > 0)
        {
            PlayerInventoryManager.Instance.AddItemByID(itemBase.ID, 1);
            PlayerInventoryManager.Instance.RemoveMoney(itemBase.price);
            shopItem.itemCount--;
            shopSlots[Array.FindIndex(shopSlots, x => x.ItemBase == itemBase)].SetValue(shopItem.itemCount, itemBase);
            moneyText.text = PlayerInventoryManager.Instance.Money.ToString();

            DataPersistenceManager.Instance.SaveGame();
        }
        else
        {
            Debug.Log("Not enough money or no item to sell");
        }
    }


    public void Activate(string shopID, LocalizedString shopName)
    {
        gameObject.SetActive(true);
        descriptionObj.SetActive(false);    
        UI_Manager.Instance.SetFirstSelectedObj(firstSelectedObj);
        UI_Manager.Instance.ResetAllInput();

        moneyText.text = PlayerInventoryManager.Instance.Money.ToString();

        GameManager.Instance.PauseGame();
        this.shopName.StringReference = shopName;

        if(shopsData == null)
        {
            Debug.LogError("shopsData == null");
            return;
        }

        if (!shopsData.ContainsKey(shopID))
        {
            GetDefaultShopData(shopID);
        }
        else
        {
            GetShopDataFromGameData(shopID);
        }

        //Load shop inventory
        int count = 0;
        foreach (var item in inventory)
        {
            SO_ItemsBase itembase = ItemDataManager.Instance.TryGetItemFromAllDict(item.Value.itemdataID);
            int quantity = item.Value.itemCount;

            if(itembase == null)
            {
                Debug.LogError("Item not found in itemDataManager");
                continue;
            }

            shopSlots[count].SetValue(quantity, itembase);

            count++;
        }

        for(int i = count; i < shopSlots.Length; i++)
        {
            shopSlots[i].Deactvate();
        }
    }

    private void GetDefaultShopData(string shopID)
    {
        ItemDataManager.Instance.ShopDict.TryGetValue(shopID, out var shop);

        if (shop == null)
        {
            Debug.LogError("Shop not found");
            return;
        }

        // Add to shopsData
        ShopInventory shopInventory = new();

        foreach (var item in shop.shopInventory)
        {
            shopInventory.shopInventory.Add(item.item.ID, new ItemData(item.quantity, item.item.ID));
        }

        shopsData.Add(shopID, shopInventory);
        inventory = shopInventory.shopInventory;

        Debug.Log("First time shop inventory created");

        // Save to file
        DataPersistenceManager.Instance.SaveGame();
    }

    private void GetShopDataFromGameData(string shopID)
    {
        shopsData.TryGetValue(shopID, out var shopData);

        if (shopData == null)
        {
            Debug.LogError("shopInventory == null");
            return;
        }

        inventory = shopData.shopInventory;

        Debug.Log("Shop inventory loaded from data.");
    }
    
    public void Deactivate()
    {
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
        UI_Manager.Instance.FirstSelectedObjNull();

        foreach (var slot in shopSlots)
        {
            slot.Deactvate();
        }
    }

    private void OnDestroy()
    {
        foreach (var slot in shopSlots)
        {
            slot.OnEnterTarget -= OnPointerEnter;
            slot.OnClick -= OnShopSlotClick;
            slot.OnExitTarget -= OnPointerExit;
        }
    }

    public void LoadData(GameData data)
    {
        shopsData = data.shopsData;
    }

    public void SaveData(GameData data)
    {
        data.shopsData = shopsData;
    }

    #region Description
    private void OnPointerEnter(LocalizedString name, LocalizedString description, int price)
    {
        descriptionName.StringReference = name;
        descriptionText.StringReference = description;
        descriptionPriceText.text = price.ToString();
        descriptionObj.SetActive(true);
    }

    private void OnPointerExit()
    {
        descriptionObj.SetActive(false);
    }
    #endregion
}
