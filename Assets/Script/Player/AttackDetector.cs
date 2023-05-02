using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("ApplyDamage", 1);
        }
    }
}
