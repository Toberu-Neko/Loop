using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class PickupHUD : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private LocalizeStringEvent localizedText;
    [SerializeField] private LocalizedString defaultText;

    private Queue<ItemText> queue = new();

    private void Awake()
    {
        queue = new();
    }

    public void AddToQueue(LocalizedString itemName)
    {
        queue.Enqueue(new ItemText { itemName = itemName});

        if (queue.Count == 1)
        {
            Activate(itemName);
        }
    }

    public void Activate(LocalizedString itemName)
    {
        localizedText.StringReference = itemName;
        anim.SetTrigger("Activate");
    }

    public void Deactivate()
    {
        queue.Dequeue();

        if (queue.Count > 0)
        {
            var itemText = queue.Peek();
            Activate(itemText.itemName);
        }
    }

    private class ItemText
    {
        public LocalizedString itemName;
    }
}
