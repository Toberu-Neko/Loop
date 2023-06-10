using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    private GameObject canvas;
    private TextMeshProUGUI lifeText;
    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        lifeText = canvas.transform.Find("Debug/LifeText").GetComponent<TextMeshProUGUI>();
    }
    public void UpdateLifeText(float life)
    {
        lifeText.text = "Life: " + life;
    }
}
