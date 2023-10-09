using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChargeTrigger : MonoBehaviour
{
    [SerializeField] Combat combat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
            combat.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            combat.RemoveFromDetected(collision);
    }
}
