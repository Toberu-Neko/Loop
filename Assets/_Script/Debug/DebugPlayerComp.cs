using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerComp : MonoBehaviour
{
    [SerializeField] GameObject PerfectBlockAttack;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(!Stats.PerfectBlockAttackable && PerfectBlockAttack.activeInHierarchy)
        {
            PerfectBlockAttack.SetActive(false);
        }
    }
}
