using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public event Action OnHealthZero;

    public int SwordEnergy { get; private set; }


    public bool PerfectBlockAttackable { get; private set; }
    [SerializeField] private float perfectBlockDuration;
    private float perfectBlockStartTime;

    private Combat Combat => combat ? combat : core.GetCoreComponent<Combat>();
    private Combat combat;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
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

        if(Time.time >= perfectBlockStartTime + perfectBlockDuration && PerfectBlockAttackable)
        {
            PerfectBlockAttackable = false;
        }
    }

    public void DecreaseHeakth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            OnHealthZero?.Invoke();
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void SetPerfectBlockAttackTrue()
    {
        perfectBlockStartTime = Time.time;
        PerfectBlockAttackable = true;
    }

    public void SetPerfectBlockAttackFalse()
    {
        PerfectBlockAttackable = false;
    }
}
