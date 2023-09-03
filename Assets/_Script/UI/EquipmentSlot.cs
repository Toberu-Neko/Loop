using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private EquipmentType equipmentType;
    private enum EquipmentType
    {
        Sword,
        Gun,
        Fist
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            return;
        }

        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();

        if ((equipmentType == EquipmentType.Sword && draggableItem.CanEquipOnSword)
            ||
            (equipmentType == EquipmentType.Gun && draggableItem.CanEquipOnGun)
            ||
            (equipmentType == EquipmentType.Fist && draggableItem.CanEquipOnFist))
        {
            draggableItem.ParentAfterDrag = transform;
            draggableItem.ReturnToOriginalParent = false;
        }
    }
}
