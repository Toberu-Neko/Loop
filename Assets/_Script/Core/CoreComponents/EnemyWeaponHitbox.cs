using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHitbox : CoreComponent
{
    private Combat combat;
    protected override void Awake()
    {
        base.Awake();

        combat = core.GetCoreComponent<Combat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            combat.AddToDetected(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player lost");
            combat.RemoveFromDetected(collision);
        }
    }
}
