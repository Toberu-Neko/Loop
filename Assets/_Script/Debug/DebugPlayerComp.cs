using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugPlayerComp : MonoBehaviour
{
    [SerializeField] GameObject perfectBlockAttack;
    [SerializeField] TextMeshProUGUI HpText;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI medkitText;

    private PlayerWeaponManager weaponManager;
    private PlayerTimeSkillManager timeSkillManager;

    public event Action<float, float, float> OnInit;
    public event Action<float> OnUpdateHp;
    public event Action<string, float> OnUpdateTimeSkill;
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
            PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].OnValueChanged += UpdateMedkitText;
        }
        InitBars();
        UpdateMedkitText();
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
            PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].OnValueChanged -= UpdateMedkitText;
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

    void UpdateMedkitText()
    {
        medkitText.text = "血包" + PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].itemCount.ToString();
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
        OnUpdateTimeSkill?.Invoke(timeSkillManager.StateMachine.CurrentState.ToString()[16..], timeSkillManager.CurrentEnergy);

        timeText.text = "裝備技能: " + timeSkillManager.StateMachine.CurrentState.ToString()[16..] +
            "\n 能量: " + timeSkillManager.CurrentEnergy.ToString();
    }

}
