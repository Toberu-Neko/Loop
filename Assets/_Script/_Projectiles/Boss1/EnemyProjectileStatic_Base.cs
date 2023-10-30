using System;
using UnityEngine;

public class EnemyProjectileStatic_Base : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] protected Core core;

    [SerializeField] private float explodeTime;
    protected Stats stats;

    private float startTime;
    private bool exploded;

    protected event Action OnExplode;

    protected virtual void Awake()
    {
        stats = core.GetCoreComponent<Stats>();
    }

    protected virtual void OnEnable()
    {
        startTime = Time.time;
        exploded = false;
    }

    protected virtual void Update()
    {
        startTime = stats.Timer(startTime);

        if(Time.time >= startTime + explodeTime && !exploded)
        {
            exploded = true;
            OnExplode?.Invoke();
        }
    }

}
