using UnityEngine;
using TMPro;
using System;
using UnityEngine.Localization;

public class DebugPlayerComp : MonoBehaviour
{
    [SerializeField] GameObject perfectBlockAttack;
    [SerializeField] TextMeshProUGUI HpText;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] LocalizedString medkitLocalizedText;
    [SerializeField] TextMeshProUGUI medkitText;

    private PlayerWeaponManager weaponManager;
    private PlayerTimeSkillManager timeSkillManager;

    public event Action<float, float, float> OnInit;
    public event Action<float> OnUpdateHp;
    public event Action<LocalizedString, float, bool> OnUpdateTimeSkill;
    public event Action<WeaponType, int, float> OnUpdateWeapon;

    private Core core;

    // private Stats Stats => stats ? stats : stats = core.GetCoreComponent<Stats>();
    private Stats stats;

    // private Combat Combat => combat ? combat : combat = core.GetCoreComponent<Combat>();
    private Combat combat;
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        stats = core.GetCoreComponent<Stats>();
        combat = core.GetCoreComponent<Combat>();

        weaponManager = GetComponent<PlayerWeaponManager>();
        timeSkillManager = GetComponent<PlayerTimeSkillManager>();

        perfectBlockAttack.SetActive(false);
    }

    void Start()
    {
        if (PlayerInventoryManager.Instance.ConsumablesInventory.ContainsKey("Medkit"))
        {
            //TODO: Do it better
            PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].OnValueChanged += ChangeMedkitCount;
            medkitLocalizedText.Arguments = new object[] { PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].itemCount };
            medkitLocalizedText.StringChanged += UpdateMedkitText;
        }
        else
        {
            Invoke(nameof(CheckInventory), 0.1f);
        }
        InitBars();
        ChangeMedkitCount();
        UpdateHpText();
        UpdateWeaponText();
    }

    private void OnEnable()
    {
        combat.OnPerfectBlock += () => perfectBlockAttack.SetActive(true);

        stats.Health.OnValueChanged += UpdateHpText;
        combat.OnDamaged += UpdateHpText;
        weaponManager.OnEnergyChanged += UpdateWeaponText;
        weaponManager.OnWeaponChanged += UpdateWeaponText;
        timeSkillManager.OnStateChanged += UpdateTimeSkillText;

    }

    private void OnDisable()
    {
        if (PlayerInventoryManager.Instance.ConsumablesInventory.ContainsKey("Medkit"))
        {
            //TODO: Do it better
            PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].OnValueChanged -= ChangeMedkitCount;
            medkitLocalizedText.StringChanged -= UpdateMedkitText;
        }

        combat.OnPerfectBlock -= () => perfectBlockAttack.SetActive(true);
        stats.Health.OnValueChanged -= UpdateHpText;
        combat.OnDamaged -= UpdateHpText;
        weaponManager.OnEnergyChanged -= UpdateWeaponText;
        weaponManager.OnWeaponChanged -= UpdateWeaponText;
        timeSkillManager.OnStateChanged -= UpdateTimeSkillText;
    }

    void Update()
    {
        if(!stats.CounterAttackable && perfectBlockAttack.activeInHierarchy)
        {
            perfectBlockAttack.SetActive(false);
        }
    }

    private void ChangeMedkitCount()
    {
        medkitLocalizedText.Arguments = new object[] { PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].itemCount };
        medkitLocalizedText.RefreshString();
    }

    void UpdateMedkitText(string value)
    {
        medkitText.text = value;
    }

    private void CheckInventory()
    {
        if (PlayerInventoryManager.Instance.ConsumablesInventory.ContainsKey("Medkit"))
        {
            PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].OnValueChanged += ChangeMedkitCount;
            medkitLocalizedText.Arguments = new object[] { PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].itemCount };
            medkitLocalizedText.StringChanged += UpdateMedkitText;
        }
        else
        {
            Invoke(nameof(CheckInventory), 0.1f);
        }
    }

    private void InitBars()
    {
        OnInit?.Invoke(stats.Health.MaxValue, timeSkillManager.MaxEnergy, weaponManager.GunData.maxEnergy);
    }
    private void UpdateHpText()
    {
        OnUpdateHp?.Invoke(stats.Health.CurrentValue);
        HpText.text =  "\n生命值: " + stats.Health.CurrentValue.ToString();
    }

    void UpdateWeaponText()
    {
        OnUpdateWeapon?.Invoke(weaponManager.CurrentWeaponType, weaponManager.GetCurrentTypeEnergy(), weaponManager.GunCurrentNormalAttackEnergy);

        if (weaponManager.CurrentWeaponType != WeaponType.Gun)
        {
            weaponText.text = "武器: " + weaponManager.CurrentWeaponType.ToString() +
                "\n 能量: " + weaponManager.GetCurrentTypeEnergy();
        }
        else
        {
            weaponText.text = "武器: " + weaponManager.CurrentWeaponType.ToString() +
                "\n 能量: " + weaponManager.GetCurrentTypeEnergy() + ", 手榴彈: " + weaponManager.GunCurrentEnergy;
        }
    }

    void UpdateTimeSkillText()
    {
        if(timeSkillManager.StateMachine.CurrentState.SkillName == timeSkillManager.StateMachine.CurrentState.Data.noneSkillName)
        {
            OnUpdateTimeSkill?.Invoke(timeSkillManager.StateMachine.CurrentState.SkillName, timeSkillManager.CurrentEnergy, false);
        }
        else
        {
            Debug.Log(timeSkillManager.StateMachine.CurrentState.SkillName.GetLocalizedString());
            OnUpdateTimeSkill?.Invoke(timeSkillManager.StateMachine.CurrentState.SkillName, timeSkillManager.CurrentEnergy, true);
        }

        timeText.text = "裝備技能: " + timeSkillManager.StateMachine.CurrentState.ToString()[16..] +
            "\n 能量: " + timeSkillManager.CurrentEnergy.ToString();
    }

}
