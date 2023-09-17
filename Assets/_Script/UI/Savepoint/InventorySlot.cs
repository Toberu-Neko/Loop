using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject draggablePrefab;
    [SerializeField] private TextMeshProUGUI countText;

    private DraggableItem previousItem;
    private DraggableItem currentItem;
    public LootSO LootSO { get; private set; }
    private List<DraggableItem> draggableItems;

    public event Action<string, string> OnEnterTarget;
    public event Action OnExitTarget;

    private bool canOpenDescription;

    public int Count { get; private set; }

    

    private void Awake()
    {
        draggableItems = new();
        canOpenDescription = true;
    }

    public void SetCount(int count)
    {
        Count = count;
        countText.text = Count.ToString();
    }

    public void SetValue(int count, LootSO so)
    {
        SetCount(count);
        LootSO = so;
        SpawnDraggableObj(LootSO);
    }

    private void SpawnDraggableObj(LootSO so)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(draggablePrefab, spawnPoint);
        obj.transform.localPosition = Vector3.zero;
        DraggableItem script = obj.GetComponent<DraggableItem>();
        previousItem = currentItem;
        currentItem = script;

        script.OnReturnToOriginalParent += HandleNoTarget;
        script.OnStartDragging += HandleDragStart;
        script.OnEndDragging += HandleEndDragging;

        script.SetValue(so, Count);
        draggableItems.Add(script);
    }


    private void HandleExit() 
    {
        OnExitTarget?.Invoke();
    }

    private void HandleDragStart()
    {
        if (Count <= 0)
        {
            Debug.LogError("Count is 0");
            return;
        }
        currentItem.OnStartDragging -= HandleDragStart;
        canOpenDescription = false;

        HandleExit();

        if (previousItem != null)
        {
            previousItem.OnReturnToOriginalParent -= HandleNoTarget;
        }

        Count--;
        countText.text = Count.ToString();
        SpawnDraggableObj(LootSO);
    }

    public void ActiveSlot()
    {
        spawnPoint.gameObject.SetActive(true);
        countText.gameObject.SetActive(true);
    }

    public void DeactiveSlot()
    {
        spawnPoint.gameObject.SetActive(false);
        countText.gameObject.SetActive(false);

        if(draggableItems.Count > 0)
        {
            foreach(var item in draggableItems)
            {
                item.OnReturnToOriginalParent -= HandleNoTarget;
                item.OnStartDragging -= HandleDragStart;
                item.Deactivate();
            }
            draggableItems.Clear();
        }
    }

    private void HandleNoTarget(DraggableItem item)
    {
        Count++;
        countText.text = Count.ToString();

        item.OnReturnToOriginalParent -= HandleNoTarget;
        item.OnStartDragging -= HandleDragStart;

        draggableItems.Remove(item);
    }

    private void HandleEndDragging(DraggableItem item)
    {
        item.OnEndDragging -= HandleEndDragging;
        item.OnReturnToOriginalParent -= HandleNoTarget;

        canOpenDescription = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(canOpenDescription)
            OnEnterTarget?.Invoke(LootSO.itemDetails.lootName, LootSO.itemDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitTarget?.Invoke();
    }
}
