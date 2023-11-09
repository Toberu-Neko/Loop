using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNum : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;

    public void Init(float amount)
    {
        damageText.text = amount.ToString();
    }

    public void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
