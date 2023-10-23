using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObj : BreakableWall
{
    [SerializeField] private GameObject particlePrefab;
    private void OnEnable()
    {
        OnDefeated += HandleOnDefeated;
    }

    private void OnDisable()
    {
        OnDefeated -= HandleOnDefeated;
    }

    private void HandleOnDefeated()
    {
        if(stats.IsTimeSlowed || stats.IsTimeStopped)
        {
            Debug.Log("Spawn bullets");
        }

        death.Die();
        // particleManager.StartParticles(particlePrefab, transform.position, Quaternion.identity);


    }
}
