using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupHUD : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    private Queue<ItemText> queue = new();

    private void Awake()
    {
        queue = new();
    }

    public void AddToQueue(string itemName, string shortItemDescription)
    {
        queue.Enqueue(new ItemText { itemName = itemName, shortItemDescription = shortItemDescription });

        if (queue.Count == 1)
        {
            Activate(itemName, shortItemDescription);
        }
    }

    public void Activate(string itemName, string shortItemDescription)
    {
        itemNameText.text = itemName;
        itemDescriptionText.text = "";
        anim.SetTrigger("Activate");
    }

    public void Deactivate()
    {
        queue.Dequeue();

        if (queue.Count > 0)
        {
            var itemText = queue.Peek();
            Activate(itemText.itemName, itemText.shortItemDescription);
        }
    }

    private class ItemText
    {
        public string itemName;
        public string shortItemDescription;
    }
}
