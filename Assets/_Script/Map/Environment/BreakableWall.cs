using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IMapDamageableItem
{
    [SerializeField] private int health = 1;

    public void TakeDamage(float damage)
    {
        health --;
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}
