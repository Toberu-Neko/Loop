using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayButtonHover(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayButtonClick(transform);
    }
}
