using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]
public class D_RangedAttackState : ScriptableObject
{
    public GameObject projectile;
    public float projectileDamage = 10f;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance = 12f;
    public float projectileLifeTime = 5f;
    public float projectileGravityScale = 0.2f;
    public float timeBetweenProjectiles = 0.5f;
    public float attackRadius = 0.5f;
}
