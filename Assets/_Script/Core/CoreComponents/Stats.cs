using System;
using UnityEngine;

/// <summary>
/// Health, Stamina, TimeSlow, etc.
/// </summary>
public class Stats : CoreComponent
{
    [field: SerializeField] public CoreStatSystem Health { get; private set; }
    [field: SerializeField] public CoreStatSystem Stamina { get; private set; }

    private float staminaRecoveryRate;
    private float perfectBlockAttackDuration;
    private float invincibleDurationAfterDamaged;
    private float combatTimer = 2f;
    private float lastCombatTime;

    public bool CanChangeWeapon { get; private set; } = true;
    public bool CounterAttackable { get; private set; }
    public bool InCombat { get; private set; } = false;
    public bool Attackable { get; private set; } = true;
    public bool Invincible { get; private set; } = false;
    public bool InvinvibleAfterDamaged { get; private set; } = false;

    // For Enemy
    public bool IsAngry { get; set; } = false;
    public bool Knockable { get; private set; } = true;

    public bool IsRewindingPosition { get; private set; } = false;
    public bool IsTimeStopped { get; private set; } = false;
    public bool IsTimeSlowed { get; private set; } = false;
    public float TimeSlowMultiplier { get; private set; } = 0f;

    public float DebuffActionSpeedMultiplier { get; set; } = 1f;
    public float TimeEffectMultiplier { get; set; } = 1f;

    public float AnimationSpeed
    {
        get
        {
            if (Invincible)
            {
                return orgAnimationSpeed;
            }
            else
            {
                return orgAnimationSpeed * DebuffActionSpeedMultiplier * TimeEffectMultiplier;
            }
        }
        set
        {
            orgAnimationSpeed = value;
        }
    }

    private float orgAnimationSpeed = 1f;

    public event Action OnTimeStopStart;
    public event Action OnTimeStopEnd;
    public event Action OnTimeSlowStart;
    public event Action OnTimeSlowEnd;

    public event Action<float> OnInvincibleStart;

    #region Overrides

    protected override void Awake()
    {
        base.Awake();

        Health.MaxValue = core.CoreData.maxHealth;
        Stamina.MaxValue = core.CoreData.maxStamina;
        staminaRecoveryRate = core.CoreData.staminaRecoveryRate;
        perfectBlockAttackDuration = core.CoreData.perfectBlockAttackDuration;
        invincibleDurationAfterDamaged = core.CoreData.invincibleDurationAfterDamaged;
        combatTimer = core.CoreData.combatTimer;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (IsTimeStopped || IsTimeSlowed)
        {
            lastCombatTime += Time.deltaTime;
        }
        if (InCombat && Time.time >= lastCombatTime + combatTimer)
        {
            InCombat = false;
        }

        if (!InCombat && !Stamina.CurrentValue.Equals(Stamina.MaxValue))
        {
            Stamina.Increase(staminaRecoveryRate * Time.deltaTime);
        }
    }


    private void OnEnable()
    {
        #region Initialize
        InvinvibleAfterDamaged = false;
        Invincible = false;
        InCombat = false;
        CanChangeWeapon = true;
        Attackable = true;
        IsRewindingPosition = false;
        IsTimeStopped = false;
        IsTimeSlowed = false;
        Knockable = true;
        IsAngry = false;
        DebuffActionSpeedMultiplier = 1f;
        TimeEffectMultiplier = 1f;

        Health.Init();
        Stamina.Init();
        #endregion

        Stamina.OnCurrentValueZero += HandlePoiseZero;
    }

    private void OnDisable()
    {
        SetTimeSlowFalse();
        SetTimeStopFalse();

        OnTimeStopEnd?.Invoke();
        OnTimeSlowEnd?.Invoke();

        Stamina.OnCurrentValueZero -= HandlePoiseZero;
    }
    #endregion

    #region PerfectBlockAttack
    public void SetPerfectBlockAttackTrue()
    {
        CounterAttackable = true;

        CancelInvoke(nameof(SetPerfectBlockAttackFalse));
        Invoke(nameof(SetPerfectBlockAttackFalse), perfectBlockAttackDuration);
    }

    public void SetPerfectBlockAttackFalse()
    {
        if(CounterAttackable)
            CounterAttackable = false;
    }
    #endregion

    #region Invincible

    public void SetInvincibleTrue()
    {
        Invincible = true;
    }

    public void SetInvincibleFalse()
    {
        Invincible = false;
    }

    public void SetInvincibleTrueAfterDamaged()
    {
        SetInvincibleAfterDamageTrue();
        CancelInvoke(nameof(SetInvincibleAfterDamageFalse));
        Invoke(nameof(SetInvincibleAfterDamageFalse), invincibleDurationAfterDamaged);
    }

    private void SetInvincibleAfterDamageTrue()
    {
        InvinvibleAfterDamaged = true;
        OnInvincibleStart?.Invoke(invincibleDurationAfterDamaged);
    }

    private void SetInvincibleAfterDamageFalse()
    {
        InvinvibleAfterDamaged = false;
    }
    #endregion

    #region EventHandler
    public void HandleOnDamaged()
    {
        InCombat = true;
        lastCombatTime = Time.time;
    }

    private void HandlePoiseZero()
    {
        Stamina.decreaseable = false;
    }
    #endregion

    #region TimeStop
    public void SeTimeStopTrue()
    {
        InCombat = true;
        lastCombatTime = Time.time;
        IsTimeStopped = true;
        TimeEffectMultiplier = 0f;

        OnTimeStopStart?.Invoke();
    }

    public void SetTimeStopFalse()
    {
        IsTimeStopped = false;
        TimeEffectMultiplier = 1f;

        OnTimeStopEnd?.Invoke();
    }
    #endregion

    #region TimeSlow
    public void SetTimeSlowTrue()
    {
        InCombat = true;
        lastCombatTime = Time.time;
        IsTimeSlowed = true;

        TimeEffectMultiplier = GameManager.Instance.TimeSlowMultiplier;
        TimeSlowMultiplier = GameManager.Instance.TimeSlowMultiplier;
        OnTimeSlowStart?.Invoke();
    }

    public void SetTimeSlowFalse()
    {
        IsTimeSlowed = false;
        TimeEffectMultiplier = 1f;
        OnTimeSlowEnd?.Invoke();
    }
    #endregion

    public void SetAttackable(bool volume) => Attackable = volume;
    public void SetCanChangeWeapon(bool volume) => CanChangeWeapon = volume;
    public void ResetPoiseDecreaseable() => Stamina.decreaseable = true;
    public void SetRewindingPosition(bool volume) => IsRewindingPosition = volume;

    public float Timer(float timer)
    {
        if (IsTimeStopped)
        {
            timer += Time.deltaTime;
            return timer;
        }

        if (IsTimeSlowed)
        {
            timer += Time.deltaTime * (1f - GameManager.Instance.TimeSlowMultiplier);
            return timer;
        }
        return timer;
    }
}
