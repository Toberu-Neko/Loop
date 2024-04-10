using System;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public WeaponType CurrentWeaponType { get; private set; }

    [field: SerializeField] public SO_WeaponData_Sword SwordData { get; private set; }
    [SerializeField] private GameObject swordEnhanceObj;
    public int SwordCurrentEnergy { get; private set; }
    public bool EnhanceSwordAttack { get; private set; }

    [field: SerializeField] public SO_WeaponData_Fist FistData { get; private set; }
    public int FistCurrentEnergy { get; private set; }


    [field: SerializeField] public SO_WeaponData_Gun GunData { get; private set; }
    [field: SerializeField] public GunChargeAttackScript GunChargeAttackScript { get; private set; }


    [field: SerializeField] public Transform ProjectileStartPos { get; private set; }
    public float GunCurrentNormalAttackEnergy { get; private set; }
    public int GunCurrentEnergy { get; private set; }
    public bool GunNormalAttackEnergyRegenable { get; private set; }
    
    
    public event Action OnEnergyChanged;
    public event Action OnWeaponChanged;

    private bool perfectBlockThisFram = false;

    private Core core;
    private Combat combat;
    private Player player;
    private Stats stats;
    private PlayerInputHandler inputHandler;
    private PlayerTimeSkillManager timeSkillManager;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        combat = core.GetCoreComponent<Combat>();
        stats = core.GetCoreComponent<Stats>();
        player = GetComponent<Player>();
        inputHandler = GetComponent<PlayerInputHandler>();
        timeSkillManager = GetComponent<PlayerTimeSkillManager>();
        GunChargeAttackScript.gameObject.SetActive(false);
    }

    private void Start()
    {
        InitializeEnergy();
    }
    private void OnEnable()
    {
        combat.OnPerfectBlock += () => perfectBlockThisFram = true;
        DataPersistenceManager.Instance.OnLoad += InitWeapon;
        swordEnhanceObj.SetActive(false);
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

        if (perfectBlockThisFram)
        {
            IncreaseEnergy();
        }

        if(CurrentWeaponType == WeaponType.Gun && GunNormalAttackEnergyRegenable)
        {
            IncreaseGunEnergy();
        }

        if (player.InputHandler.DebugInput)
        {
            IncreaseAllEnergy();
            timeSkillManager.SetTimeEnergyMax();
            stats.Health.Increase(50f);
        }
    }

    public void SetEnhanceSwordAttack(bool value)
    {
        EnhanceSwordAttack = value;
        swordEnhanceObj.SetActive(value);
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        CurrentWeaponType = weaponType;

        OnWeaponChanged?.Invoke();
    }

    private void InitWeapon()
    {
        CurrentWeaponType = PlayerInventoryManager.Instance.EquipedWeapon[0];
        OnWeaponChanged?.Invoke();
    }

    private void ChangeWeapon()
    {
        if (inputHandler.ChangeWeapon2 && PlayerInventoryManager.Instance.CanUseWeaponCount > 1)
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
        GunCurrentNormalAttackEnergy = GunData.maxEnergy;
        GunCurrentEnergy = 0;
        GunNormalAttackEnergyRegenable = true;
        EnhanceSwordAttack = false;
    }

    public int GetCurrentTypeEnergy()
    {
        switch (CurrentWeaponType)
        {
            case WeaponType.Sword:
                return SwordCurrentEnergy;
            case WeaponType.Fist:
                return FistCurrentEnergy;
            case WeaponType.Gun:
                return GunCurrentEnergy;
            case WeaponType.None:
                return 0;
            default:
                Debug.LogError("No Weapon Type");
                return 0;
        }
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
                if(GunCurrentEnergy < GunData.maxGrenade)
                    GunCurrentEnergy++;
                break;
            default:
                break;
        }
        perfectBlockThisFram = false;
        OnEnergyChanged?.Invoke();
    }

    private void IncreaseGunEnergy()
    {
        if (GunCurrentNormalAttackEnergy < GunData.maxEnergy)
            GunCurrentNormalAttackEnergy += GunData.energyRegen * Time.deltaTime;

        if (GunCurrentNormalAttackEnergy > GunData.maxEnergy)
            GunCurrentNormalAttackEnergy = GunData.maxEnergy;

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
                GunCurrentEnergy--;
                break;
            default:
                break;
        }
        OnEnergyChanged?.Invoke();
    }

    public void DecreaseGunNormalAttackEnergy()
    {
        GunCurrentNormalAttackEnergy -= GunData.energyCostPerShot;
        OnEnergyChanged?.Invoke();
    }

    public void DecreaseGunNormalAttackEnergy(float amount)
    {
        if(GunCurrentNormalAttackEnergy < amount)
            GunCurrentNormalAttackEnergy = 0;
        else
            GunCurrentNormalAttackEnergy -= amount;

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
            default:
                break;
        }
        OnEnergyChanged?.Invoke();
    }

    private void IncreaseAllEnergy()
    {
        if(SwordCurrentEnergy < SwordData.maxEnergy)
            SwordCurrentEnergy++;

        if(FistCurrentEnergy < FistData.maxEnergy)
            FistCurrentEnergy++;

        if(GunCurrentEnergy < GunData.maxGrenade)
            GunCurrentEnergy++;

        GunCurrentNormalAttackEnergy = GunData.maxEnergy;

        OnEnergyChanged?.Invoke();
    }

    public void SetGunRegenable(bool regenable)
    {
        GunNormalAttackEnergyRegenable = regenable;
    }

    public void GunFiredRegenDelay()
    {
        GunNormalAttackEnergyRegenable = false;
        CancelInvoke(nameof(SetGunRagenableTrue));
        Invoke(nameof(SetGunRagenableTrue), GunData.energyRegenDelay);
    }

    private void SetGunRagenableTrue() => GunNormalAttackEnergyRegenable = true;

}

