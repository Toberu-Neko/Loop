using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject draggablePrefab;
    [SerializeField] private TextMeshProUGUI countText;
    private List<DraggableItem> draggableItems;
    private Sprite sprite;
    private DraggableItem currentDraggableItem;

    public int Count { get; private set; }

    private void Awake()
    {
        draggableItems = new();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void SetValue(int count, Sprite img)
    {
        Count = count;
        countText.text = Count.ToString();
        sprite = img;

        SpawnDraggableObj();
    }

    private void SpawnDraggableObj()
    {
        GameObject obj = ObjectPoolManager.SpawnObject(draggablePrefab, spawnPoint);
        obj.transform.localPosition = Vector3.zero;
        DraggableItem script = obj.GetComponent<DraggableItem>();
        currentDraggableItem = script;
        script.OnReturnToOriginalParent += HandleReturnObject;
        script.OnStartDragging += HandleDragStart;
        script.SetValue(sprite);
        draggableItems.Add(script);
    }

    private void HandleDragStart()
    {
        Count--;
        countText.text = Count.ToString();
        SpawnDraggableObj();
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

        if(currentDraggableItem != null)
            currentDraggableItem.Deactivate();
    }

    private void HandleReturnObject(DraggableItem item)
    {
        Count++;
        countText.text = Count.ToString();

        item.OnReturnToOriginalParent -= HandleReturnObject;
        item.OnStartDragging -= HandleDragStart;

        draggableItems.Remove(item);
    }

}
