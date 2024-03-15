using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNum : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;

    public void Init(float amount)
    {
        string amountString;
        if (amount < 1)
        {
            amountString = string.Format("{0:N2}", amount);
        }
        else
        {
            amountString = string.Format("{0:N0}", amount);
        }

        damageText.text = amountString;
    }

    public void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
