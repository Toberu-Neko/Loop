using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button == null)
        {
            return;
        }

        if (!button.interactable)
        {
            return;
        }
        AudioManager.instance.PlayButtonHover(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button == null)
        {
            return;
        }

        if (!button.interactable)
        {
            Debug.LogWarning("Button is not interactable");
            return;
        }

        AudioManager.instance.PlayButtonClick(transform);
    }
}
