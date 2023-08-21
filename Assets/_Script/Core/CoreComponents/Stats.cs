using System;
using UnityEngine;

public class Stats : CoreComponent
{
    [field: SerializeField] public CoreStatSystem Health { get; private set; }
    [field: SerializeField] public CoreStatSystem Stamina { get; private set; }

    private float staminaRecoveryRate;
    private float perfectBlockAttackDuration;
    private float invincibleDurationAfterDamaged;
    private float combatTimer = 2f;
    private float lastCombatTime;


    public bool PerfectBlockAttackable { get; private set; }
    public bool Invincible { get; private set; } = false;
    public bool InCombat { get; private set; } = false;
    public bool CanChangeWeapon { get; private set; } = true;
    public bool Attackable { get; private set; } = true;

    public bool IsRewindingPosition { get; private set; } = false;
    public bool IsTimeStopped { get; private set; } = false;
    public bool IsTimeSlowed { get; private set; } = false;
    public float TimeSlowMultiplier { get; private set; } = 0f;
    public float AnimationSpeed { get; private set; } = 1f;

    public event Action OnTimeStopStart;
    public event Action OnTimeStopEnd;
    public event Action OnTimeSlowStart;
    public event Action OnTimeSlowEnd;

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
        Invincible = false;
        InCombat = false;
        CanChangeWeapon = true;
        Attackable = true;
        IsRewindingPosition = false;
        IsTimeStopped = false;
        IsTimeSlowed = false;

        Health.Init();
        Stamina.Init();

        Stamina.OnCurrentValueZero += HandlePoiseZero;
    }

    private void OnDisable()
    {

        SetTimeSlowFalse();
        SetTimeStopFalse();

        Stamina.OnCurrentValueZero -= HandlePoiseZero;
    }
    #endregion

    #region PerfectBlockAttack
    public void SetPerfectBlockAttackTrue()
    {
        PerfectBlockAttackable = true;

        CancelInvoke(nameof(SetPerfectBlockAttackFalse));
        Invoke(nameof(SetPerfectBlockAttackFalse), perfectBlockAttackDuration);
    }

    public void SetPerfectBlockAttackFalse()
    {
        if(PerfectBlockAttackable)
            PerfectBlockAttackable = false;
    }
    #endregion

    #region Invincible

    public void SetInvincibleTrue()
    {
        Invincible = true;
        Physics2D.IgnoreLayerCollision(7, 11, true);
        Physics2D.IgnoreLayerCollision(7, 13, true);
    }

    public void SetInvincibleFalse()
    {
        Invincible = false;
        Physics2D.IgnoreLayerCollision(7, 11, false);
        Physics2D.IgnoreLayerCollision(7, 13, false);
    }

    public void SetInvincibleTrueAfterDamaged()
    {
        SetInvincibleTrue();
        CancelInvoke(nameof(SetInvincibleFalse));
        Invoke(nameof(SetInvincibleFalse), invincibleDurationAfterDamaged);
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
        Stamina.Init();
        Stamina.decreaseable = false;
    }
    #endregion

    #region TimeStop
    public void SeTimeStopTrue()
    {
        InCombat = true;
        lastCombatTime = Time.time;
        IsTimeStopped = true;
        OnTimeStopStart?.Invoke();
    }

    public void SetTimeStopFalse()
    {
        IsTimeStopped = false;
        OnTimeStopEnd?.Invoke();
    }
    #endregion

    #region TimeSlow
    public void SetTimeSlowTrue()
    {
        InCombat = true;
        lastCombatTime = Time.time;
        IsTimeSlowed = true;
        TimeSlowMultiplier = GameManager.Instance.TimeSlowMultiplier;
        OnTimeSlowStart?.Invoke();
    }

    public void SetTimeSlowFalse()
    {
        IsTimeSlowed = false;
        OnTimeSlowEnd?.Invoke();
    }
    #endregion

    public void SetAttackable(bool volume) => Attackable = volume;
    public void SetCanChangeWeapon(bool volume) => CanChangeWeapon = volume;
    public void ResetPoiseDecreaseable() => Stamina.decreaseable = true;
    public void SetRewindingPosition(bool volume) => IsRewindingPosition = volume;
}
