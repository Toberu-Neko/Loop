using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject damageParticles;

    private Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    private Stats Stats => stats ? stats : core.GetCoreComponent<Stats>();
    private Stats stats;

    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent<ParticleManager>();
    private ParticleManager particleManager;

    [SerializeField]
    private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive;
    private float knockbackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float damageAmount)
    {
        Debug.Log(core.transform.parent.name + "Damaged");
        Stats?.DecreaseHeakth(damageAmount);
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        // Debug.Log(isKnockbackActive);
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            // Debug.Log("Reset");
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }
}
