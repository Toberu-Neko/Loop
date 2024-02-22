using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color emptyTextColor;

    public SO_ItemsBase ItemBase { get; private set; }

    private ShopItem spawnedItem;
    private bool canOpenDescription;

    public event Action<LocalizedString, LocalizedString, int> OnEnterTarget;
    public event Action OnExitTarget;
    public event Action<SO_ItemsBase> OnClick;

    public void SetValue(int quantity, SO_ItemsBase itemBase)
    {
        canOpenDescription = true;
        countText.gameObject.SetActive(true);

        ItemBase = itemBase;

        countText.text = quantity.ToString();
        countText.color = quantity > 0 ? normalTextColor : emptyTextColor;

        //Spawn item prefab
        if (spawnedItem != null)
        {
            spawnedItem.ReturnToPool();
        }

        GameObject spawnedObj = ObjectPoolManager.SpawnObject(shopItemPrefab, spawnPoint);
        spawnedItem = spawnedObj.GetComponent<ShopItem>();

        spawnedItem.SetValue(itemBase.itemSprite);
    }

    public void Deactvate()
    {
        if(spawnedItem != null)
            spawnedItem.ReturnToPool();

        ItemBase = null;
        canOpenDescription = false;
        countText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canOpenDescription)
        {
            OnClick.Invoke(ItemBase);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canOpenDescription && ItemBase != null)
        {
            OnEnterTarget.Invoke(ItemBase.displayNameLocalization, ItemBase.descriptionLocalization, ItemBase.price);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canOpenDescription)
        {
            OnExitTarget.Invoke();
        }
    }

}
