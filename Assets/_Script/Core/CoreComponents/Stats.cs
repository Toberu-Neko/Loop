using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [field: SerializeField] public CoreStatSystem Health { get; private set; }
    [field: SerializeField] public CoreStatSystem Poise { get; private set; }
    [SerializeField] private float staminaRecoveryRate;
    [SerializeField] private float perfectBlockAttackDuration;
    [SerializeField] private float invincibleDurationAfterDamaged;

    public bool PerfectBlockAttackable { get; private set; }
    public bool Invincible { get; private set; } = false;

    public bool InCombat { get; private set; } = false;
    public bool CanChangeWeapon { get; private set; } = true;

    private Combat combat;
    [SerializeField] private float combatTimer = 2f;
    private float lastCombatTime;
    private bool damagedThisFrame = false;
    private bool staminaDamagedThisFrame = false;
    private bool knockbackedThisFrame = false;

    protected override void Awake()
    {
        base.Awake();

        combat = core.GetCoreComponent<Combat>();

        Health.Init();
        Poise.Init();
    }
    private void Update()
    {
        if(InCombat && Time.time >= lastCombatTime + combatTimer)
        {
            InCombat = false;
        }

        if (!InCombat && !Poise.CurrentValue.Equals(Poise.MaxValue))
        {
            Poise.Increase(staminaRecoveryRate * Time.deltaTime);
        }

        if(damagedThisFrame && knockbackedThisFrame && staminaDamagedThisFrame)
        {
            SetInvincibleTrueAfterDamaged();
            damagedThisFrame = false;
            knockbackedThisFrame = false;
            staminaDamagedThisFrame = false;
        }
    }
    private void OnEnable()
    {
        combat.OnPerfectBlock += SetPerfectBlockAttackTrue;
        combat.OnDamaged += HandleOnDamaged;
        Poise.OnCurrentValueZero += HandlePoiseZero;

        combat.OnDamaged += () => damagedThisFrame = true;
        combat.OnKnockback += () => knockbackedThisFrame = true;
        combat.OnStaminaDamaged += () => staminaDamagedThisFrame = true;
    }

    private void OnDisable()
    {
        combat.OnPerfectBlock -= SetPerfectBlockAttackTrue;
        combat.OnDamaged -= HandleOnDamaged;
        Poise.OnCurrentValueZero -= HandlePoiseZero;

        combat.OnDamaged -= () => damagedThisFrame = true;
        combat.OnKnockback -= () => knockbackedThisFrame = true;
        combat.OnStaminaDamaged -= () => staminaDamagedThisFrame = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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

    public void SetInvincibleTrue() => Invincible = true;

    public void SetInvincibleFalse() => Invincible = false;

    private void SetInvincibleTrueAfterDamaged()
    {
        Invincible = true;
        CancelInvoke(nameof(SetInvincibleFalse));
        Invoke(nameof(SetInvincibleFalse), invincibleDurationAfterDamaged);
    }

    public void SetCanChangeWeapon(bool volume) => CanChangeWeapon = volume;

    private void HandleOnDamaged()
    {

        InCombat = true;
        lastCombatTime = Time.time;
    }

    private void HandlePoiseZero()
    {
        Poise.Init();
        Poise.decreaseable = false;
    }

    public void ResetPoiseDecreaseable() => Poise.decreaseable = true;
}
