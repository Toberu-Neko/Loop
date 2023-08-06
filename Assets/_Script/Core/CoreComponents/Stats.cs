using System;
using System.Collections;
using System.Collections.Generic;
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

    public event Action OnTimeStopStart;
    public event Action OnTimeStopEnd;
    public event Action OnTimeSlowStart;
    public event Action OnTimeSlowEnd;

    private void Start()
    {

        Health.MaxValue = core.CoreData.maxHealth;
        Stamina.MaxValue = core.CoreData.maxStamina;
        staminaRecoveryRate = core.CoreData.staminaRecoveryRate;
        perfectBlockAttackDuration = core.CoreData.perfectBlockAttackDuration;
        invincibleDurationAfterDamaged = core.CoreData.invincibleDurationAfterDamaged;
        combatTimer = core.CoreData.combatTimer;

        Health.Init();
        Stamina.Init();
    }
    private void Update()
    {
        if (IsTimeStopped)
        {
            lastCombatTime += Time.deltaTime;
        }
        if(InCombat && Time.time >= lastCombatTime + combatTimer)
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
        Stamina.OnCurrentValueZero += HandlePoiseZero;
    }

    private void OnDisable()
    {
        Stamina.OnCurrentValueZero -= HandlePoiseZero;
    }

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

    public void SetCanChangeWeapon(bool volume) => CanChangeWeapon = volume;

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

    public void ResetPoiseDecreaseable() => Stamina.decreaseable = true;

    public void SetRewindingPosition(bool volume) => IsRewindingPosition = volume;

    public void SeTimeStopTrue()
    {
        IsTimeStopped = true;
        OnTimeStopStart?.Invoke();
    }

    public void SetTimeStopFalse()
    {
        IsTimeStopped = false;
        OnTimeStopEnd?.Invoke();
    }

    public void SetTimeSlowTrue()
    {
        IsTimeSlowed = true;
        TimeSlowMultiplier = GameManager.Instance.TimeSlowMultiplier;
        OnTimeSlowStart?.Invoke();
    }

    public void SetTimeSlowFalse()
    {
        IsTimeSlowed = false;
        OnTimeSlowEnd?.Invoke();
    }

    public void SetAttackable(bool volume) => Attackable = volume;
}
