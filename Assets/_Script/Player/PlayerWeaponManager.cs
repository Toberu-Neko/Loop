using System;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public WeaponType CurrentWeaponType { get; private set; }

    [field: SerializeField] public SO_WeaponData_Sword SwordData { get; private set; }
    public int SwordCurrentEnergy { get; private set; }

    [field: SerializeField] public SO_WeaponData_Fist FistData { get; private set; }
    public int FistCurrentEnergy { get; private set; }


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
        DataPersistenceManager.Instance.OnLoad += InitWeapon;
        InitializeEnergy();
    }
    private void OnEnable()
    {
        combat.OnPerfectBlock += () => perfectBlockThisFram = true;
    }
    private void OnDisable()
    {
        combat.OnPerfectBlock -= () => perfectBlockThisFram = true;
        DataPersistenceManager.Instance.OnLoad -= InitWeapon;
    }



    private void Update()
    {
       if(stats.CanChangeWeapon)
            ChangeWeapon();

        if ((CurrentWeaponType == WeaponType.Sword || CurrentWeaponType == WeaponType.Fist) && perfectBlockThisFram)
        {
            IncreaseEnergy();
        }

        if(CurrentWeaponType == WeaponType.Gun && GunEnergyRegenable)
        {
            IncreaseEnergy();
        }

        if (player.InputHandler.DebugInput)
        {
            AllEnergyMax();
        }
    }

    private void InitWeapon()
    {
        CurrentWeaponType = PlayerInventoryManager.Instance.EquipedWeapon[0];
        OnWeaponChanged?.Invoke();
    }

    private void ChangeWeapon()
    {
        if (inputHandler.ChangeWeapon2)
        {
            inputHandler.UseChangeWeapon2();
            if(CurrentWeaponType == PlayerInventoryManager.Instance.EquipedWeapon[0])
            {
                CurrentWeaponType = PlayerInventoryManager.Instance.EquipedWeapon[1];
            }
            else
            {
                CurrentWeaponType = PlayerInventoryManager.Instance.EquipedWeapon[0];
            }

            OnWeaponChanged?.Invoke();
        }
    }

    public void InitializeEnergy()
    {
        SwordCurrentEnergy = 0;
        FistCurrentEnergy = 0;
        GunCurrentEnergy = GunData.maxEnergy;
        GunEnergyRegenable = true;
    }
    public string GetCurrentTypeEnergyStr()
    {
        switch (CurrentWeaponType)
        {
            case WeaponType.Sword:
                return SwordCurrentEnergy.ToString();
            case WeaponType.Fist:
                return FistCurrentEnergy.ToString();
            case WeaponType.Gun:
                return GunCurrentEnergy.ToString();
        }

        Debug.Log("WeaponTyperError");
        return "WeaponTyperError";
    }

    private void IncreaseEnergy()
    {
        switch (CurrentWeaponType)
        {
            case WeaponType.Sword:
                if (SwordCurrentEnergy < SwordData.maxEnergy)
                    SwordCurrentEnergy++;
                break;
            case WeaponType.Fist:
                if(FistCurrentEnergy < FistData.maxEnergy)
                    FistCurrentEnergy++;
                break;
            case WeaponType.Gun:
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
            case WeaponType.Sword:
                SwordCurrentEnergy--;
                break;
            case WeaponType.Fist:
                FistCurrentEnergy--;
                break;
            case WeaponType.Gun:
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

    public void ClearCurrentEnergy()
    {
        switch (CurrentWeaponType)
        {
            case WeaponType.Sword:
                SwordCurrentEnergy = 0;
                break;
            case WeaponType.Fist:
                FistCurrentEnergy = 0;
                break;
            case WeaponType.Gun:
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

    public void GunFiredRegenDelay()
    {
        GunEnergyRegenable = false;
        CancelInvoke(nameof(SetGunRagenableTrue));
        Invoke(nameof(SetGunRagenableTrue), GunData.energyRegenDelay);
    }
    private void SetGunRagenableTrue() => GunEnergyRegenable = true;

}

