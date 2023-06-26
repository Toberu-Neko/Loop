using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoreStatSystem
{
    public event Action OnCurrentValueZero;
    public event Action OnValueChanged;

    [field: SerializeField] public float MaxValue { get; private set; }

    public float CurrentValue
    {
        get => currentValue;
        private set
        {
            currentValue = Mathf.Clamp(value, 0, MaxValue);
            if (currentValue <= 0)
            {
                OnCurrentValueZero?.Invoke();
            }
        }
    }

    private float currentValue;

    public void Init()
    {
        CurrentValue = MaxValue;
    }

    public void Increase(float amount)
    {
        CurrentValue += amount;
        OnValueChanged?.Invoke();
    }

    public void Decrease(float amount)
    {
        CurrentValue -= amount;
        OnValueChanged?.Invoke();
    }
}
