using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveToEnemy1 : MonoBehaviour, IDamageable
{
    Enemy1 enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy1>();
    }
    public void Damage(AttackDetails details)
    {
        enemy.Damage(details);
    }
}
