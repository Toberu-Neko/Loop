using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void Active(string name, string description)
    {
        GameManager.Instance.PauseGame();
        gameObject.SetActive(true);
        itemNameText.text = name;
        itemDescriptionText.text = description;
    }

    public void Deactive()
    {
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
    }
}
