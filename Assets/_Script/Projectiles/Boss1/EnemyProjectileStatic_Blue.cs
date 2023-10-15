using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileStatic_Blue : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] protected Core core;
    protected Stats stats;

    protected virtual void Awake()
    {
        stats = core.GetCoreComponent<Stats>();
    }

}
