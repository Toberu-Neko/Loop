using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to detect if the player is hit by the enemy weapon.
/// </summary>
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
            combat.AddToDetected(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            combat.RemoveFromDetected(collision);
        }
    }
}
