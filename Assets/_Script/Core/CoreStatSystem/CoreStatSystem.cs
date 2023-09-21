using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoreStatSystem
{
    public event Action OnCurrentValueZero;
    public event Action OnValueChanged;
    public event Action OnValueDecreased;

    [field: SerializeField] public float MaxValue { get; set; }
    // public float DeltaValue { get; private set; } = 0f;

    public float CurrentValue
    {
        get => currentValue;
        private set
        {
            currentValue = Mathf.Clamp((float)value, 0f, MaxValue);
            if (currentValue <= 0)
            {
                OnCurrentValueZero?.Invoke();
            }
        }
    }

    [HideInInspector] public bool decreaseable = true;
    private float currentValue;

    public void Init()
    {
        CurrentValue = MaxValue;
        OnValueChanged?.Invoke();
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
        OnValueDecreased?.Invoke();
    }
}
