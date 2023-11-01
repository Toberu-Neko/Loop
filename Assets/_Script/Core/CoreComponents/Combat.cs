using System;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable, IStaminaDamageable, ISlowable
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
    public List<IMapDamageableItem> DetectedMapDamageableItems { get; private set; } = new();


    public bool PerfectBlockAllDir { get; private set; }
    public bool PerfectBlock { get; private set; }
    private bool normalBlock;

    //Core
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
    public event Action<float> OnGoToKinematicState;
    public event Action OnGoToStunState;

    // time
    private float staminaDelta = 0f;
    private float healthDelta = 0f;
    private float knockStrengthDelta = 0f;
    private Vector2 knockbackAngleDelta = Vector2.zero;
    private Vector2 workspace = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();

        stats = core.GetCoreComponent<Stats>();
        movement = core.GetCoreComponent<Movement>();
        particleManager = core.GetCoreComponent<ParticleManager>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();
    }
    private void Start()
    {
        damageParticles = core.CoreData.damageParticles;
        blockDamageMultiplier = core.CoreData.blockDamageMultiplier;
        blockStaminaMultiplier = core.CoreData.blockStaminaMultiplier;
        maxKnockbackTime = core.CoreData.maxKnockbackTime;
        normalBlockKnockbakDirection = core.CoreData.normalBlockKnockbakDirection;
        normalBlockKnockbakMultiplier = core.CoreData.normalBlockKnockbakMultiplier;
    }

    private void OnEnable()
    {
        workspace = Vector2.zero;
        knockbackAngleDelta = Vector2.zero;
        knockStrengthDelta = 0f;
        healthDelta = 0f;
        staminaDelta = 0f;
        isKnockbackActive = false;
        knockbackStartTime = 0f;
        damagedThisFrame = false;
        PerfectBlock = false;
        normalBlock = false;
        PerfectBlockAllDir = false;

        DetectedDamageables = new();
        DetectedKnockbackables = new();
        DetectedStaminaDamageables = new();
        DetectedMapDamageableItems = new();


        stats.OnTimeStopEnd += HandleStartTime;
        stats.OnTimeSlowStart += HandleTimeSlowStart;
        stats.OnTimeSlowEnd += HandleTimeSlowEnd;

        OnPerfectBlock += HandlePerfectBlock;
        OnDamaged += HandleOnDamaged;
    }

    private void OnDisable()
    {
        stats.OnTimeStopEnd -= HandleStartTime;
        stats.OnTimeSlowStart -= HandleTimeSlowStart;
        stats.OnTimeSlowEnd -= HandleTimeSlowEnd;
        OnPerfectBlock -= HandlePerfectBlock;
        OnDamaged -= HandleOnDamaged;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isKnockbackActive)
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
            knockStrengthDelta *= -1;

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

    public void SetPerfectBlockAllDir(bool value)
    {
        PerfectBlockAllDir = value;
    }

    private  void HandleTimeSlowStart()
    {
        maxKnockbackTime /= stats.TimeSlowMultiplier;
    }
    private void HandleTimeSlowEnd()
    {
        maxKnockbackTime *= stats.TimeSlowMultiplier;
    }
    #endregion

    #region Stamina
    public void TakeStaminaDamage(float damageAmount, Vector2 damagePosition, bool blockable)
    {
        if (stats.Invincible || stats.InvinvibleAfterDamaged || !stats.Stamina.decreaseable)
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
        if (stats.Invincible || stats.InvinvibleAfterDamaged)
        {
            return;
        }
        else if (!blockable)
        {
            DecreaseHealth(damageAmount);
        }
        else if (PerfectBlock || PerfectBlockAllDir)
        {
            if (PerfectBlockAllDir)
            {
                OnPerfectBlock?.Invoke();
            }
            else if (!FacingDamgePosition(damagePosition))
            {
                DecreaseHealth(damageAmount);
            }
            else
            {
                OnPerfectBlock?.Invoke();
            }
        }
        else if(normalBlock)
        {
            // Debug.Log("normalBlock");
            DecreaseHealth(damageAmount * blockDamageMultiplier);
        }
        else
        {
            // Debug.Log("else");
            DecreaseHealth(damageAmount);
        }
    }

    public GameObject GetGameObject()
    {
        return movement.ParentTransform.gameObject;
    }

    public void GotoKinematicState(float time = -1)
    {
        OnGoToKinematicState?.Invoke(time);
    }

    public void GoToStunState()
    {
        OnGoToStunState?.Invoke();
    }

    private void DecreaseHealth(float damageAmount)
    {
        if (stats.IsTimeStopped)
        {
            healthDelta += damageAmount;
            return;
        }

        OnDamaged?.Invoke();

        if (damageAmount > 0)
        {
            stats.Health.Decrease(damageAmount);
            particleManager.StartParticlesWithRandomRotation(damageParticles);

            if (movement.ParentTransform.gameObject.CompareTag("Player"))
            {
                CamManager.Instance.CameraShake();
            }
        }
    }
    #endregion

    #region Knockback

    public void Knockback(Vector2 angle, float strength, Vector2 damagePosition, bool blockable = true)
    {
        angle = angle.normalized;
        int direction;

        if(damagePosition.x - movement.ParentTransform.position.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        if (stats.Invincible || stats.InvinvibleAfterDamaged || !stats.Knockable)
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
    }


    private void HandleKnockback(float strength, Vector2 angle, int direction)
    {
        if(strength == 0f || angle == Vector2.zero)
        {
            return;
        }

        OnKnockback?.Invoke();

        if (stats.IsTimeStopped)
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

        if(movement.RB.bodyType == RigidbodyType2D.Kinematic)
        {
            movement.SetKnockbackDynamic();
        }

        movement.SetVelocity(strength, angle, direction);
        movement.SetCanSetVelocity(false);
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }


    private void CheckKnockback()
    {
        if (isKnockbackActive && ((movement.CurrentVelocity.y <= 0.01f && collisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            movement.SetCanSetVelocity(true);

            movement.SetKnockbackKinematic();

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
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            DetectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Add(knockbackable);
        }

        if(collision.TryGetComponent(out IStaminaDamageable staminaDamageable))
        {
            DetectedStaminaDamageables.Add(staminaDamageable);
        }

        if(collision.TryGetComponent(out IMapDamageableItem mapDamageableItem))
        {
            DetectedMapDamageableItems.Add(mapDamageableItem);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            DetectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent(out IKnockbackable knockbackable))
        {
            DetectedKnockbackables.Remove(knockbackable);
        }

        if(collision.TryGetComponent(out IStaminaDamageable staminaDamageable))
        {
            DetectedStaminaDamageables.Remove(staminaDamageable);
        }

        if(collision.TryGetComponent(out IMapDamageableItem mapDamageableItem))
        {
            DetectedMapDamageableItems.Remove(mapDamageableItem);
        }
    }
    #endregion

    private float tempMovementMultiplier;

    public void SetDebuffMultiplier(float multiplier, float delayTime = 0f)
    {
        if(multiplier <= stats.DebuffActionSpeedMultiplier)
        {
            stats.DebuffActionSpeedMultiplier = multiplier;

            CancelInvoke(nameof(SetDebuffMultiplierOne));

            if(delayTime > 0f)
                Invoke(nameof(SetDebuffMultiplierOne), delayTime);
        }
    }

    public void SetDebuffMultiplierOne()
    {
        stats.DebuffActionSpeedMultiplier = 1f;
    }

}
