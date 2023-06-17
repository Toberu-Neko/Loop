using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private float blockDamageMultiplier = 0.5f;

    public event Action OnDamaged;

    public bool PerfectBlock { get; set; }
    public bool NormalBlock { get; set; }

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

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable)
    {
        OnDamaged?.Invoke();

        if (!blockable)
        {
            Stats?.DecreaseHeakth(damageAmount);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
            return;
        }

        if (PerfectBlock)
        {
            Debug.Log("PerfectBlock!");
            return;
        }

        if (NormalBlock)
        {
            Debug.Log("NormalBlock!");
            Stats?.DecreaseHeakth(damageAmount * blockDamageMultiplier);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
            return;
        }

        Stats?.DecreaseHeakth(damageAmount * blockDamageMultiplier);
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void Knockback(Vector2 angle, float strength, int direction, bool blockable = true)
    {
        if(!blockable)
        {
            Movement?.SetVelocity(strength, angle, direction);
            Movement.CanSetVelocity = false;

            isKnockbackActive = true;
            knockbackStartTime = Time.time;
            return;
        }

        if (PerfectBlock)
        {
            return;
        }

        if (NormalBlock)
        {
            return;
        }

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
