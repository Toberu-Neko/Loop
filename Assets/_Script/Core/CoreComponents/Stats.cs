using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [field: SerializeField] public CoreStatSystem Health { get; private set; }
    [field: SerializeField] public CoreStatSystem Stamina { get; private set; }
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

    float am = 0;
    protected override void Awake()
    {
        base.Awake();

        combat = core.GetCoreComponent<Combat>();
    }
    private void Update()
    {
        if(InCombat && Time.time >= lastCombatTime + combatTimer)
        {
            InCombat = false;
        }

        if (!InCombat && !Stamina.CurrentValue.Equals(Stamina.MaxValue))
        {
            Stamina.Increase(staminaRecoveryRate * Time.deltaTime);
            // am += Time.deltaTime;
            // Debug.Log(am);
        }
    }
    private void LateUpdate()
    {
        if (damagedThisFrame)
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
        Stamina.OnCurrentValueZero += HandlePoiseZero;

        combat.OnDamaged += () => damagedThisFrame = true;
        combat.OnKnockback += () => knockbackedThisFrame = true;
        combat.OnStaminaDamaged += () => staminaDamagedThisFrame = true;
    }

    private void OnDisable()
    {
        combat.OnPerfectBlock -= SetPerfectBlockAttackTrue;
        combat.OnDamaged -= HandleOnDamaged;
        Stamina.OnCurrentValueZero -= HandlePoiseZero;

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

    public void SetInvincibleTrue()
    {
        Invincible = true;
        Physics2D.IgnoreLayerCollision(7, 11, true);
    }

    public void SetInvincibleFalse()
    {
        Invincible = false;
        Physics2D.IgnoreLayerCollision(7, 11, false);
    }

    private void SetInvincibleTrueAfterDamaged()
    {
        SetInvincibleTrue();
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
        Stamina.Init();
        Stamina.decreaseable = false;
    }

    public void ResetPoiseDecreaseable() => Stamina.decreaseable = true;
}
