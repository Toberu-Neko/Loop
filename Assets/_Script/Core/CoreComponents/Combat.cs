using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Combat : CoreComponent, IDamageable, IKnockbackable, IStaminaDamageable
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private float blockDamageMultiplier = 0.5f;

    public event Action OnPerfectBlock;
    public event Action OnDamaged;
    public event Action OnKnockback;
    public event Action OnStaminaDamaged;

    public List<IDamageable> DetectedDamageables { get; private set; } = new();
    public List<IKnockbackable> DetectedKnockbackables { get; private set; } = new();
    public List<IStaminaDamageable> DetectedStaminaDamageables { get; private set; } = new();

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

    [SerializeField] private float maxKnockbackTime = 0.2f;
    [SerializeField] private Vector2 normalBlockKnockbakDirection = new(1, 0.25f);
    [SerializeField] private float normalBlockKnockbakMultiplier = 0.75f;


    private bool isKnockbackActive;
    private float knockbackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }
    public void TakeStaminaDamage(float damageAmount, Vector2 damagePosition, bool blockable)
    {
        if (Stats.Invincible || !Stats.Poise.decreaseable)
        {
            return;
        }
        else if (!blockable || !FacingDamgePosition(damagePosition))
        {
            Stats.Poise.Decrease(damageAmount);
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if (NormalBlock)
        {
            Stats.Poise.Decrease(damageAmount * blockDamageMultiplier);
        }
        else
        {
            Stats.Poise.Decrease(damageAmount);
        }
        OnStaminaDamaged?.Invoke();
    }

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable)
    {
        if (Stats.Invincible)
        {
            return;
        }
        else if (!blockable || !FacingDamgePosition(damagePosition))
        {
            Stats.Health.Decrease(damageAmount);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if(NormalBlock)
        {
            Stats.Health.Decrease(damageAmount * blockDamageMultiplier);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        }
        else
        {
            Stats.Health.Decrease(damageAmount);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        }
        OnDamaged?.Invoke();
    }


    public void Knockback(Vector2 angle, float strength, int direction, Vector2 damagePosition, bool blockable = true)
    {
        if (Stats.Invincible)
        {
            return;
        }
        else if (!blockable || !FacingDamgePosition(damagePosition))
        {
            Movement.SetVelocity(strength, angle, direction);
            StartKnockback();
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if (NormalBlock)
        {
            Movement.SetVelocity(strength * normalBlockKnockbakMultiplier, normalBlockKnockbakDirection, direction);
            StartKnockback();
        }
        else
        {
            Movement.SetVelocity(strength, angle, direction);
            StartKnockback();
        }
        OnKnockback?.Invoke();
    }

    private void StartKnockback()
    {
        Movement.CanSetVelocity = false;
        // Movement.SetDragZero();
        // Movement.SetGravityZero();

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    public bool FacingDamgePosition(Vector2 damagePosition)
    {
        int damageDirection;
        if (damagePosition.x - core.transform.position.x > 0)
        {
            damageDirection = 1;
        }
        else
        {
            damageDirection = -1;
        }

        return damageDirection == Movement.FacingDirection;
    }

    private void CheckKnockback()
    {
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            Movement.CanSetVelocity = true;
            // Movement.SetDragOrginal();
            // Movement.SetGravityOrginal();
            isKnockbackActive = false;
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            DetectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Add(knockbackable);
        }

        if(collision.TryGetComponent<IStaminaDamageable>(out IStaminaDamageable staminaDamageable))
        {
            DetectedStaminaDamageables.Add(staminaDamageable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            DetectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Remove(knockbackable);
        }

        if(collision.TryGetComponent<IStaminaDamageable>(out IStaminaDamageable staminaDamageable))
        {
            DetectedStaminaDamageables.Remove(staminaDamageable);
        }
    }


}
