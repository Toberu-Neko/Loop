using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerWeaponType CurrentWeaponType { get; private set; }

    [Header("Sword")]
    [SerializeField, HideInInspector] private int nothing;
    [field: SerializeField] public SO_WeaponData_Sword SwordData { get; private set; }
    public int SwordCurrentEnergy { get; private set; }
    public int FistCurrentEnergy { get; private set; }
    public int GunCurrentEnergy { get; private set; }

    public event Action OnEnergyChanged;
    public event Action OnWeaponChanged;

    private bool perfectBlockThisFram = false;

    private Core core;
    private Combat combat;
    private Player player;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        combat = core.GetCoreComponent<Combat>();
        player = GetComponent<Player>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        CurrentWeaponType = PlayerWeaponType.Sword;
        InitializeEnergy();
    }

    private void Update()
    {
        if (inputHandler.ChangeWeapon1 && CurrentWeaponType != PlayerWeaponType.Sword)
        {
            CurrentWeaponType = PlayerWeaponType.Sword;
            OnWeaponChanged?.Invoke();
        }
        else if (inputHandler.ChangeWeapon2 && CurrentWeaponType != PlayerWeaponType.Fist)
        {
            CurrentWeaponType = PlayerWeaponType.Fist;
            OnWeaponChanged?.Invoke();
        }
        else if (inputHandler.ChangeWeapon3 && CurrentWeaponType != PlayerWeaponType.Gun)
        {
            CurrentWeaponType = PlayerWeaponType.Gun;
            OnWeaponChanged?.Invoke();
        }

        if (perfectBlockThisFram)
        {
            IncreaseEnergy();
        }
    }
    public void InitializeEnergy()
    {
        SwordCurrentEnergy = 0;
        FistCurrentEnergy = 0;
        GunCurrentEnergy = 100;

    }
    public int GetCurrentTypeEnergy()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                return SwordCurrentEnergy;
            case PlayerWeaponType.Fist:
                return FistCurrentEnergy;
            case PlayerWeaponType.Gun:
                return GunCurrentEnergy;
        }
        return -1;
    }

    private void IncreaseEnergy()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                if (SwordCurrentEnergy < SwordData.maxEnergy)
                    SwordCurrentEnergy++;
                break;
            case PlayerWeaponType.Fist:
                break;
            case PlayerWeaponType.Gun:
                break;
        }
        perfectBlockThisFram = false;
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
        combat.OnPerfectBlock += () => perfectBlockThisFram = true ;
    }
    private void OnDisable()
    {
        combat.OnPerfectBlock -= () => perfectBlockThisFram = true;
    }
}

public enum PlayerWeaponType
{
    Sword,
    Fist,
    Gun
}
