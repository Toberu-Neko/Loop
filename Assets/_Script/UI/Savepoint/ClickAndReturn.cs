using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClickAndReturn : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;
    public event Action OnReturn;

    public void SetValue(SO_Chip so)
    {
        image.sprite = so.itemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnReturn?.Invoke();
    }
}
