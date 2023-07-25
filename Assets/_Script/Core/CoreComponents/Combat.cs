using System;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable, IStaminaDamageable
{
    private GameObject damageParticles;
    private float blockDamageMultiplier = 0.5f;
    private float blockStaminaMultiplier = 0.5f;

    private float maxKnockbackTime = 0.2f;
    private Vector2 normalBlockKnockbakDirection = new(1, 0.25f);
    private float normalBlockKnockbakMultiplier = 0.75f;

    public List<IDamageable> DetectedDamageables { get; private set; } = new();
    public List<IKnockbackable> DetectedKnockbackables { get; private set; } = new();
    public List<IStaminaDamageable> DetectedStaminaDamageables { get; private set; } = new();

    public bool PerfectBlock { get; private set; }
    private bool normalBlock;

    //Core
    // private Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;
    private CollisionSenses collisionSenses;
    private Stats stats;
    private ParticleManager particleManager;

    private bool isKnockbackActive;
    private float knockbackStartTime;
    private bool damagedThisFrame = false;

    // events
    public event Action OnPerfectBlock;
    public event Action OnDamaged;
    public event Action OnKnockback;
    public event Action OnStaminaDamaged;

    // time
    private float staminaDelta = 0f;
    private float healthDelta = 0f;
    private float knockStrengthDelta = 0f;
    private Vector2 knockbackAngleDelta = Vector2.zero;
    private Vector2 workspace = Vector2.zero;

    private void Start()
    {
        stats = core.GetCoreComponent<Stats>();
        movement = core.GetCoreComponent<Movement>();
        particleManager = core.GetCoreComponent<ParticleManager>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();

        damageParticles = core.CoreData.damageParticles;
        blockDamageMultiplier = core.CoreData.blockDamageMultiplier;
        blockStaminaMultiplier = core.CoreData.blockStaminaMultiplier;
        maxKnockbackTime = core.CoreData.maxKnockbackTime;
        normalBlockKnockbakDirection = core.CoreData.normalBlockKnockbakDirection;
        normalBlockKnockbakMultiplier = core.CoreData.normalBlockKnockbakMultiplier;

        stats.OnTimeStart += HandleStartTime;
        OnPerfectBlock += HandlePerfectBlock;
        OnDamaged += HandleOnDamaged;
    }
    private void OnDisable()
    {
        stats.OnTimeStart -= HandleStartTime;
        OnPerfectBlock -= HandlePerfectBlock;
        OnDamaged -= HandleOnDamaged;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckKnockback();
    }

    public override void LateLogicUpdate()
    {
        base.LateLogicUpdate();

        if (damagedThisFrame)
        {
            stats.SetInvincibleTrueAfterDamaged();
            damagedThisFrame = false;
        }
    }

    #region Event Handlers
    private void HandleStartTime()
    {
        DecreaseHealth(healthDelta);
        healthDelta = 0f;

        DecreaseStamina(staminaDelta);
        staminaDelta = 0f;

        int dir;
        if (knockStrengthDelta < 0)
        {
            dir = -1;
            knockStrengthDelta *= 1;

        }
        else
        {
            dir = 1;
        }

        HandleKnockback(knockStrengthDelta, knockbackAngleDelta, dir);

        knockStrengthDelta = 0f;
        knockbackAngleDelta = Vector2.zero;
    }

    private void HandlePerfectBlock()
    {
        stats.SetPerfectBlockAttackTrue();
    }

    private void HandleOnDamaged()
    {
        stats.HandleOnDamaged();
        damagedThisFrame = true;
    }
    #endregion

    #region Stamina
    public void TakeStaminaDamage(float damageAmount, Vector2 damagePosition, bool blockable)
    {
        if (stats.Invincible || !stats.Stamina.decreaseable)
        {
            return;
        }
        else if (!blockable || !FacingDamgePosition(damagePosition))
        {
            DecreaseStamina(damageAmount);
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if (normalBlock)
        {
            DecreaseStamina(damageAmount * blockStaminaMultiplier);
        }
        else
        {
            DecreaseStamina(damageAmount);
        }
        OnStaminaDamaged?.Invoke();
    }
    private void DecreaseStamina(float amount)
    {
        if(stats.IsTimeStopped)
        {
            staminaDelta += amount;
            return;
        }
        stats.Stamina.Decrease(amount);
    }
    #endregion

    #region Damage

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable)
    {
        if (stats.Invincible)
        {
            return;
        }
        else if (!blockable || !FacingDamgePosition(damagePosition))
        {
            DecreaseHealth(damageAmount);

            particleManager.StartParticlesWithRandomRotation(damageParticles);
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if(normalBlock)
        {
            DecreaseHealth(damageAmount * blockDamageMultiplier);

            particleManager.StartParticlesWithRandomRotation(damageParticles);
        }
        else
        {
            DecreaseHealth(damageAmount);

            particleManager.StartParticlesWithRandomRotation(damageParticles);
        }
        OnDamaged?.Invoke();
    }

    private void DecreaseHealth(float damageAmount)
    {
        if (stats.IsTimeStopped)
        {
            healthDelta += damageAmount;
            return;
        }

        if(damageAmount == 0f)
        {
            return;
        }

        stats.Health.Decrease(damageAmount);
        particleManager.StartParticlesWithRandomRotation(damageParticles);
    }
    #endregion

    #region Knockback

    public void Knockback(Vector2 angle, float strength, int direction, Vector2 damagePosition, bool blockable = true)
    {
        if (stats.Invincible)
        {
            return;
        }
        else if (!blockable || !FacingDamgePosition(damagePosition))
        {
            HandleKnockback(strength, angle, direction);
        }
        else if (PerfectBlock)
        {
            OnPerfectBlock?.Invoke();
        }
        else if (normalBlock)
        {
            HandleKnockback(strength * normalBlockKnockbakMultiplier, normalBlockKnockbakDirection, direction);
        }
        else
        {
            HandleKnockback(strength, angle, direction);
        }
        OnKnockback?.Invoke();
    }

    private void HandleKnockback(float strength, Vector2 angle, int direction)
    {
        if(stats.IsTimeStopped)
        {
            if(knockbackAngleDelta.x < angle.x)
            {
                workspace.Set(angle.x, knockbackAngleDelta.y);
                knockbackAngleDelta = workspace;
            }
            if(knockbackAngleDelta.y < angle.y)
            {
                workspace.Set(knockbackAngleDelta.x, angle.y);
                knockbackAngleDelta = workspace;
            }

            knockStrengthDelta += strength * direction;
            return;
        }

        movement.SetVelocity(strength, angle, direction);
        movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if (isKnockbackActive && ((movement.CurrentVelocity.y <= 0.01f && collisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            movement.CanSetVelocity = true;
            isKnockbackActive = false;
        }
    }
    #endregion

    #region Block
    public void SetPerfectBlock(bool value)
    {
        PerfectBlock = value;
    }

    public void SetNormalBlock(bool value)
    {
        normalBlock = value;
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

        return damageDirection == movement.FacingDirection;
    }
    #endregion

    #region Attack
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
    #endregion

}
