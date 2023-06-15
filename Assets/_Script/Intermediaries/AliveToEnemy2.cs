using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveToEnemy2 : MonoBehaviour, IDamageable
{
    Enemy2 enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy2>();
    }
    public void Damage(AttackDetails details)
    {
        enemy.Damage(details);
    }
}
