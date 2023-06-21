using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float perfectBlockAttackDuration;

    public float CurrentHealth { get; private set; }

    public event Action OnHealthZero;

    public bool PerfectBlockAttackable { get; private set; }
    public bool Invincible { get; private set; }
    // private float perfectBlockStartTime;

    private Combat Combat => combat ? combat : core.GetCoreComponent<Combat>();
    private Combat combat;

    protected override void Awake()
    {
        base.Awake();

        CurrentHealth = maxHealth;
    }

    private void OnEnable()
    {
        Combat.OnPerfectBlock += SetPerfectBlockAttackTrue;

    }

    private void OnDisable()
    {
        Combat.OnPerfectBlock -= SetPerfectBlockAttackTrue;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public void DecreaseHeakth(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            
            OnHealthZero?.Invoke();
        }
    }

    public void IncreaseHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
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
