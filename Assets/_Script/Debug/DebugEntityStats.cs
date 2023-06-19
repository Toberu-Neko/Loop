using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugEntityStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    private Core core;

    private Movement Movement => movement ? movement : movement = core.GetCoreComponent<Movement>();
    private Movement movement;
    private Stats Stats => stats ? stats : stats = core.GetCoreComponent<Stats>();
    private Stats stats;

    private Combat Combat => combat ? combat : combat = core.GetCoreComponent<Combat>();
    private Combat combat;

    private Camera cam;
    private void Awake()
    {
        core = GetComponentInParent<Core>();
        cam = Camera.main;
    }
    void Start()
    {
        UpdateHPText();
    }

    private void Update()
    {
        if(transform.rotation != cam.transform.rotation)
        {
            transform.rotation = cam.transform.rotation;
        }
    }

    private void OnEnable()
    {
        Combat.OnDamaged += UpdateHPText;
    }

    private void OnDisable()
    {
        Combat.OnDamaged -= UpdateHPText;
    }

    void UpdateHPText() => hpText.text = "HP: " + Stats.CurrentHealth.ToString();
}
