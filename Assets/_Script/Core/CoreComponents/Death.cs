using System;
using UnityEngine;

/// <summary>
/// This component is responsible for handling the death of the object, when death animation finished. And it will trigger the death event.
/// </summary>
public class Death : CoreComponent
{
    public bool IsDead { get; private set; }

    private GameObject[] deathParticles;
    public event Action OnDeath;

    private ParticleManager particleManager;

    protected override void Awake()
    {
        base.Awake();

        particleManager = core.GetCoreComponent<ParticleManager>();
    }

    private void OnEnable()
    {
        IsDead = false;
    }

    private void Start()
    {
        deathParticles = core.CoreData.deathParticles;
    }


    public void Die()
    {
        if (IsDead)
        {
            Debug.LogError("Trying to kill a dead object");
            return;
        }
        IsDead = true;
        OnDeath?.Invoke();

        foreach (var particle in deathParticles)
        {
            particleManager.StartParticles(particle);
        }

        core.transform.parent.gameObject.SetActive(false);
    }
}
