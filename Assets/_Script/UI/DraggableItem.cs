using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image image;
    [HideInInspector] public Transform ParentAfterDrag { get; set; }

    [Header("Equipment Type")]
    [SerializeField, HideInInspector] private bool nothing;
    [field: SerializeField] public bool CanEquipOnSword { get; private set; }
    [field: SerializeField] public bool CanEquipOnGun { get; private set; }
    [field: SerializeField] public bool CanEquipOnFist { get; private set; }

    private bool isCopy;

    public bool ReturnToOriginalParent { get; set; } = true;

    public event Action<DraggableItem> OnReturnToOriginalParent;
    public event Action OnStartDragging;


    private void OnEnable()
    {
        ReturnToOriginalParent = true;
        image.raycastTarget = true;
        isCopy = false;
    }

    public void SetValue(Sprite img)
    {
        image.sprite = img;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnStartDragging?.Invoke();

        isCopy = true;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ReturnToOriginalParent)
        {
            OnReturnToOriginalParent?.Invoke(this);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            // Destroy(gameObject);
            return;
        }
        transform.SetParent(ParentAfterDrag);
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCopy)
        {
            OnReturnToOriginalParent?.Invoke(this);
            gameObject.transform.SetParent(transform.root);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            // Destroy(gameObject);
        }
    }

    public void Deactivate()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
