using UnityEngine;

public class ParticleManager : CoreComponent
{

    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
    {
        return ObjectPoolManager.SpawnObject(particlePrefab, position, rotation, ObjectPoolManager.PoolType.ParticleSystem);
    }

    public GameObject StartParticles(GameObject particlePrefab)
    {
        return StartParticles(particlePrefab, transform.position, Quaternion.identity);
    }

    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab)
    {
        var randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        return StartParticles(particlePrefab, transform.position, randomRotation);
    }
    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab, Vector2 position)
    {
        var randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        return StartParticles(particlePrefab, position, randomRotation);
    }
}
