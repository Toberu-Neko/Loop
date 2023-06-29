using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxToCoreCombat : MonoBehaviour
{
    private Combat combat;
    BoxCollider boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
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

    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
