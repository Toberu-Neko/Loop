using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFirstSelecter : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private Button firstSelectedButton;

    protected virtual void OnEnable()
    {
        SetFirstSelected(firstSelectedButton);
    }

    public void SetFirstSelected(Button selectedButton)
    {
        selectedButton.Select();
    }
}
