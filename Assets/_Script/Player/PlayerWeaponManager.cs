using System;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerWeaponType CurrentWeaponType { get; private set; }

    [Header("Sword")]
    [SerializeField, HideInInspector] private int nothing;
    [field: SerializeField] public SO_WeaponData_Sword SwordData { get; private set; }
    public int SwordCurrentEnergy { get; private set; }

    [Header("Fist")]
    [SerializeField, HideInInspector] private int nothing2;
    [field: SerializeField] public SO_WeaponData_Fist FistData { get; private set; }
    public int FistCurrentEnergy { get; private set; }


    [Header("Gun")]
    [SerializeField, HideInInspector] private int nothing3;
    [field: SerializeField] public SO_WeaponData_Gun GunData { get; private set; }
    [field: SerializeField] public GunChargeAttackScript GunChargeAttackScript { get; private set; }


    [field: SerializeField] public Transform ProjectileStartPos { get; private set; }
    public float GunCurrentEnergy { get; private set; }
    public bool GunEnergyRegenable { get; private set; }
    
    
    public event Action OnEnergyChanged;
    public event Action OnWeaponChanged;

    private bool perfectBlockThisFram = false;

    private Core core;
    private Combat combat;
    private Player player;
    private Stats stats;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        combat = core.GetCoreComponent<Combat>();
        stats = core.GetCoreComponent<Stats>();
        player = GetComponent<Player>();
        inputHandler = GetComponent<PlayerInputHandler>();
        GunChargeAttackScript.gameObject.SetActive(false);
    }

    private void Start()
    {
        CurrentWeaponType = PlayerWeaponType.Sword;
        InitializeEnergy();
    }

    private void Update()
    {
       if(stats.CanChangeWeapon)
            ChangeWeapon();

        if ((CurrentWeaponType == PlayerWeaponType.Sword || CurrentWeaponType == PlayerWeaponType.Fist) && perfectBlockThisFram)
        {
            IncreaseEnergy();
        }

        if(CurrentWeaponType == PlayerWeaponType.Gun && GunEnergyRegenable)
        {
            IncreaseEnergy();
        }

        if (player.InputHandler.DebugInput)
        {
            AllEnergyMax();
        }
    }

    private void ChangeWeapon()
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
    }

    public void InitializeEnergy()
    {
        SwordCurrentEnergy = 0;
        FistCurrentEnergy = 5;
        GunCurrentEnergy = GunData.maxEnergy;
        GunEnergyRegenable = true;
    }
    public string GetCurrentTypeEnergyStr()
    {
        switch (CurrentWeaponType)
        {
            case PlayerWeaponType.Sword:
                return SwordCurrentEnergy.ToString();
            case PlayerWeaponType.Fist:
                return FistCurrentEnergy.ToString();
            case PlayerWeaponType.Gun:
                return GunCurrentEnergy.ToString();
        }

        Debug.Log("WeaponTyperError");
        return "WeaponTyperError";
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
                if(FistCurrentEnergy < FistData.maxEnergy)
                    FistCurrentEnergy++;
                break;
            case PlayerWeaponType.Gun:
                if(GunCurrentEnergy < GunData.maxEnergy)
                    GunCurrentEnergy += GunData.energyRegen * Time.deltaTime;

                if (GunCurrentEnergy > GunData.maxEnergy)
                    GunCurrentEnergy = GunData.maxEnergy;
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
                FistCurrentEnergy--;
                break;
            case PlayerWeaponType.Gun:
                GunCurrentEnergy -= GunData.energyCostPerShot;
                break;
        }
        OnEnergyChanged?.Invoke();
    }

    public void DecreaseGunEnergy(float amount)
    {
        if(GunCurrentEnergy < amount)
            GunCurrentEnergy = 0;
        else
            GunCurrentEnergy -= amount;

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
                FistCurrentEnergy = 0;
                break;
            case PlayerWeaponType.Gun:
                GunCurrentEnergy = 0;
                break;
        }
        OnEnergyChanged?.Invoke();
    }

    private void AllEnergyMax()
    {
        SwordCurrentEnergy = SwordData.maxEnergy;
        FistCurrentEnergy = FistData.maxEnergy;
        GunCurrentEnergy = GunData.maxEnergy;

        OnEnergyChanged?.Invoke();
    }

    public void SetGunRegenable(bool regenable)
    {
        GunEnergyRegenable = regenable;
    }

    public void GunFired()
    {
        GunEnergyRegenable = false;
        CancelInvoke(nameof(SetGunRagenableTrue));
        Invoke(nameof(SetGunRagenableTrue), GunData.energyRegenDelay);
    }
    private void SetGunRagenableTrue() => GunEnergyRegenable = true;

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
