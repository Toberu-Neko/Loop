using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerWeaponType CurrentWeaponType { get; private set; }

    [Header("Sword")]
    [SerializeField, HideInInspector] private int nothing;
    [field: SerializeField] public SO_WeaponData_Sword SwordData { get; private set; }
    public int SwordCurrentEnergy { get; private set; }

    public event Action OnEnergyChanged;


    private Core core;

    private Combat Combat => combat ? combat : core.GetCoreComponent<Combat>();
    private Combat combat;
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
    }

    private void Start()
    {
        CurrentWeaponType = PlayerWeaponType.Sword;
        SwordCurrentEnergy = 0;
    }

    public int GetCurrentTypeEnergy()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                return SwordCurrentEnergy;
            case PlayerWeaponType.Fist:
                break;
            case PlayerWeaponType.Gun:
                break;
        }
        return -1;
    }

    private void IncreaseEnergy()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                if(SwordCurrentEnergy < SwordData.maxEnergy)
                    SwordCurrentEnergy++;
                break;
            case PlayerWeaponType.Fist:
                break;
            case PlayerWeaponType.Gun:
                break;
        }
        OnEnergyChanged?.Invoke();
    }

    public void DecreaseEnergy()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                SwordCurrentEnergy--;
                break;
            case PlayerWeaponType.Fist:
                break;
            case PlayerWeaponType.Gun:
                break;
        }
        OnEnergyChanged?.Invoke();
    }

    public void ClearEnergy()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                SwordCurrentEnergy = 0;
                break;
            case PlayerWeaponType.Fist:
                break;
            case PlayerWeaponType.Gun:
                break;
        }
        OnEnergyChanged?.Invoke();
    }
    private void OnEnable()
    {
        Combat.OnPerfectBlock += IncreaseEnergy;
    }
    private void OnDisable()
    {
        Combat.OnPerfectBlock -= IncreaseEnergy;
    }
}

public enum PlayerWeaponType
{
    Sword,
    Fist,
    Gun
}
