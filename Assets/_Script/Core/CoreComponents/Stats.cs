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
    public bool PerfectBlockAttackable { get; private set; }
    public bool Invincible { get; private set; } = false;

    private Combat combat;


    protected override void Awake()
    {
        base.Awake();

        combat = core.GetCoreComponent<Combat>();

        Health.Init();
        Stamina.Init();
    }
    private void Update()
    {
        if (!Stamina.CurrentValue.Equals(Stamina.MaxValue))
        {
            Stamina.Increase(staminaRecoveryRate * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        combat.OnPerfectBlock += SetPerfectBlockAttackTrue;
    }

    private void OnDisable()
    {
        combat.OnPerfectBlock -= SetPerfectBlockAttackTrue;
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
    }

    public void SetInvincibleFalse()
    {
        Invincible = false;
    }
}
