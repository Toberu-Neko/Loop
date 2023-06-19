using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private float blockDamageMultiplier = 0.5f;

    public event Action OnDamaged;
    public event Action OnPerfectBlock;

    public List<IDamageable> DetectedDamageables { get; private set; } = new();
    public List<IKnockbackable> DetectedKnockbackables { get; private set; } = new();

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
        if (!blockable || !FacingDamgePosition(damagePosition))
        {
            Stats?.DecreaseHeakth(damageAmount);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if(NormalBlock)
        {
            Stats?.DecreaseHeakth(damageAmount * blockDamageMultiplier);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        }
        else
        {
            Stats?.DecreaseHeakth(damageAmount * blockDamageMultiplier);
            ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        }
        OnDamaged?.Invoke();
    }


    public void Knockback(Vector2 angle, float strength, int direction, Vector2 damagePosition, bool blockable = true)
    {
        if (!blockable || !FacingDamgePosition(damagePosition))
        {
            Movement.SetVelocity(strength, angle, direction);
            Movement.CanSetVelocity = false;

            isKnockbackActive = true;
            knockbackStartTime = Time.time;
        }
        else if (PerfectBlock)
        {
            return;
        }
        else if (NormalBlock)
        {
            return;
        }
        else
        {
            Movement.SetVelocity(strength, angle, direction);
            Movement.CanSetVelocity = false;

            isKnockbackActive = true;
            knockbackStartTime = Time.time;
        }
    }

    private bool FacingDamgePosition(Vector2 damagePosition)
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
        // Debug.Log(isKnockbackActive);
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            // Debug.Log("Reset");
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log($"Add {collision.gameObject.transform.parent.parent.name}");
            DetectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log($"Remove {collision.gameObject.transform.parent.parent.name}");
            DetectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Remove(knockbackable);
        }
    }
}
