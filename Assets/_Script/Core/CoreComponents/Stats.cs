using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public event Action OnHealthZero;

    public bool PerfectBlockAttack { get; private set; }
    private float perfectBlockTimer;

    private Combat Combat => combat ? combat : core.GetCoreComponent<Combat>();
    private Combat combat;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
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
        // TODO: Add perfect block attack timer
        PerfectBlockAttack = true;
    }
}
