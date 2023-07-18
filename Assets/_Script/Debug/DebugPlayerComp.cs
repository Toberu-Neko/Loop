using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPlayerComp : MonoBehaviour
{
    [SerializeField] GameObject perfectBlockAttack;
    [SerializeField] TextMeshProUGUI HpText;
    [SerializeField] TextMeshProUGUI weaponText;
    [SerializeField] TextMeshProUGUI timeText;

    private PlayerWeaponManager weaponManager;
    private PlayerTimeSkillManager timeSkillManager;

    private Core core;

    private Stats Stats => stats ? stats : stats = core.GetCoreComponent<Stats>();
    private Stats stats;

    private Combat Combat => combat ? combat : combat = core.GetCoreComponent<Combat>();
    private Combat combat;
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        weaponManager = GetComponent<PlayerWeaponManager>();
        timeSkillManager = GetComponent<PlayerTimeSkillManager>();

        perfectBlockAttack.SetActive(false);
    }

    void Start()
    {
        UpdateHpText();
        UpdateWeaponText();
    }

    private void OnEnable()
    {
        Combat.OnPerfectBlock += () => perfectBlockAttack.SetActive(true);
        Stats.Health.OnValueChanged += UpdateHpText;
        Combat.OnDamaged += UpdateHpText;
        weaponManager.OnEnergyChanged += UpdateWeaponText;
        weaponManager.OnWeaponChanged += UpdateWeaponText;
        timeSkillManager.OnStateChanged += UpdateTimeSkillText;
    }

    private void OnDisable()
    {
        Combat.OnPerfectBlock -= () => perfectBlockAttack.SetActive(true);
        Stats.Health.OnValueChanged -= UpdateHpText;
        Combat.OnDamaged -= UpdateHpText;
        weaponManager.OnEnergyChanged -= UpdateWeaponText;
        weaponManager.OnWeaponChanged -= UpdateWeaponText;
        timeSkillManager.OnStateChanged -= UpdateTimeSkillText;
    }

    void Update()
    {
        if(!Stats.PerfectBlockAttackable && perfectBlockAttack.activeInHierarchy)
        {
            perfectBlockAttack.SetActive(false);
        }
    }

    void UpdateHpText() => HpText.text = "生命值: " + Stats.Health.CurrentValue.ToString();

    void UpdateWeaponText()
    {
        weaponText.text = "武器: " + weaponManager.CurrentWeaponType.ToString() +
            "\n 能量: " + weaponManager.GetCurrentTypeEnergyStr();
    }

    void UpdateTimeSkillText()
    {
        timeText.text = "裝備技能: " + timeSkillManager.StateMachine.CurrentState.ToString()[16..] +
            "\n 能量: " + timeSkillManager.CurrentEnergy.ToString();
    }
}
