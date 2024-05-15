using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private TextMeshProUGUI countText;

    public event Action<SO_ItemsBase> OnEnter;
    public event Action OnExit;

    private GameObject spawnObj;
    private SO_ItemsBase itemBase;

    public void SetItem(SO_ItemsBase itemBase, int amount = 0)
    {
        ResetObj();

        if(itemBase != null)
        {
            countText.gameObject.SetActive(true);

            this.itemBase = itemBase;
            spawnObj = ObjectPoolManager.SpawnObject(itemUIPrefab, spawnPoint);

            if(itemBase.itemSprite != null) 
                spawnObj.GetComponent<Image>().sprite = itemBase.itemSprite;

            //set count
            if (itemBase is SO_StoryItem || itemBase is SO_MovementSkillItem || itemBase is SO_WeaponItem || itemBase is SO_TimeSkillItem)
            {
                countText.gameObject.SetActive(false);
            }
            else
            {
                countText.gameObject.SetActive(true);
                countText.text = amount.ToString();
            }
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }

    public void Deactivate()
    {
        ResetObj();
        countText.gameObject.SetActive(false);
    }

    private void ResetObj()
    {
        if(spawnObj != null)
        {
            ObjectPoolManager.ReturnObjectToPool(spawnObj);
            spawnObj = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (spawnObj)
        {
            OnEnter?.Invoke(itemBase);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (spawnObj)
        {
            OnExit?.Invoke();
        }
    }
}
