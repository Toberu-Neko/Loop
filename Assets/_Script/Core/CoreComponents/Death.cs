using UnityEngine;

public class Death : CoreComponent
{
    private GameObject[] deathParticles;

    private Stats Stats => stats ? stats : core.GetCoreComponent<Stats>();
    private Stats stats;

    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent<ParticleManager>();
    private ParticleManager particleManager;

    private void Start()
    {
        deathParticles = core.CoreData.deathParticles;
    }
    public void Die()
    {
        foreach(var particle in deathParticles)
        {
            ParticleManager.StartParticles(particle);
        }

        core.transform.parent.gameObject.SetActive(false);
    }

    /*
    private void OnEnable()
    {
        TODO: Modify this if need to go dead state first.
        Stats.Health.OnCurrentValueZero += Die;
    }

    private void OnDisable()
    {
        Stats.Health.OnCurrentValueZero -= Die;
    }*/
}
