using System;
using UnityEngine;

public class Death : CoreComponent
{
    private GameObject[] deathParticles;
    public event Action OnDeath;

    private ParticleManager particleManager;

    protected override void Awake()
    {
        base.Awake();

        particleManager = core.GetCoreComponent<ParticleManager>();
    }

    private void Start()
    {
        deathParticles = core.CoreData.deathParticles;
    }

    public void Die()
    {
        OnDeath?.Invoke();

        foreach (var particle in deathParticles)
        {
            particleManager.StartParticles(particle);
        }

        core.transform.parent.gameObject.SetActive(false);
    }
}
