using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPlayerComp : MonoBehaviour
{
    [SerializeField] GameObject PerfectBlockAttack;
    [SerializeField] TextMeshProUGUI HpText;

    private Core core;

    private Stats Stats => stats ? stats : stats = core.GetCoreComponent<Stats>();
    private Stats stats;

    private Combat Combat => combat ? combat : combat = core.GetCoreComponent<Combat>();
    private Combat combat;
    private void Awake()
    {
        core = GetComponentInChildren<Core>();

        PerfectBlockAttack.SetActive(false);
    }

    void Start()
    {
        Combat.OnPerfectBlock += () => PerfectBlockAttack.SetActive(true);
        HpText.text = "生命值: " + Stats.CurrentHealth.ToString();
    }

    private void OnEnable()
    {
        Combat.OnDamaged += UpdateHpText;
    }

    private void OnDisable()
    {
        Combat.OnDamaged -= UpdateHpText;
    }

    void Update()
    {
        if(!Stats.PerfectBlockAttackable && PerfectBlockAttack.activeInHierarchy)
        {
            PerfectBlockAttack.SetActive(false);
        }
    }

    void UpdateHpText() => HpText.text = "生命值: " + Stats.CurrentHealth.ToString();
}
