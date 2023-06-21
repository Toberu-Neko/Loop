using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxToCoreCombat : MonoBehaviour
{
    private Combat combat;

    private void Start()
    {
        combat = GameObject.Find("Player").GetComponentInChildren<Combat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        combat.AddToDetected(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        combat.RemoveFromDetected(collision);
    }
}
