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

    private int count;

    public bool DontHaveTarget { get; set; } = true;
    public LootSO LootSO { get; private set;}

    public event Action<DraggableItem> OnReturnToOriginalParent;
    public event Action OnStartDragging;


    private void OnEnable()
    {
        DontHaveTarget = true;
        image.raycastTarget = true;
    }

    public void SetValue(LootSO so, int count)
    {
        this.count = count;

        image.sprite = so.lootSprite;
        LootSO = so;
        CanEquipOnSword = so.canEquipOnSword;
        CanEquipOnGun = so.canEquipOnGun;
        CanEquipOnFist = so.canEquipOnFist;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(count <= 0)
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
        if (count <= 0)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (count <= 0)
        {
            return;
        }

        if (DontHaveTarget)
        {
            OnReturnToOriginalParent?.Invoke(this);
            Deactivate();
            return;
        }
        Deactivate();
    }


    public void Deactivate()
    {
        gameObject.transform.SetParent(transform.root);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
