using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    [HideInInspector] public Transform ParentAfterDrag { get; set; }

    public bool CanEquipOnSword { get; private set; }
    public bool CanEquipOnGun { get; private set; }
    public bool CanEquipOnFist { get; private set; }

    public int Count { get; set; }

    public bool DontHaveTarget { get; set; } = true;
    public SO_Chip LootSO { get; private set;}

    public event Action<DraggableItem> OnReturnToOriginalParent;
    public event Action OnStartDragging;
    public event Action<DraggableItem> OnEndDragging;


    private void OnEnable()
    {
        DontHaveTarget = true;
        image.raycastTarget = true;
    }

    public void SetValue(SO_Chip so, int count)
    {
        this.Count = count;

        image.sprite = so.itemSprite;
        LootSO = so;
        CanEquipOnSword = so.canEquipOnSword;
        CanEquipOnGun = so.canEquipOnGun;
        CanEquipOnFist = so.canEquipOnFist;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Count <= 0)
        {
            return;
        }

        OnStartDragging?.Invoke();

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Count <= 0)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Count <= 0)
        {
            return;
        }

        if (DontHaveTarget)
        {
            OnReturnToOriginalParent?.Invoke(this);
        }
        OnEndDragging?.Invoke(this);
        Deactivate();
    }


    public void Deactivate()
    {
        gameObject.transform.SetParent(transform.root);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

}
