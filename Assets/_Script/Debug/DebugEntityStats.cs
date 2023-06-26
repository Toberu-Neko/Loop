using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugEntityStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    private Canvas canvas;
    private Core core;

    private Movement movement;
    private Stats stats;
    private Combat combat;

    private Camera cam;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        core = GetComponentInParent<Core>();
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();
        combat = core.GetCoreComponent<Combat>();

        cam = Camera.main;
        canvas.worldCamera = cam;
    }
    void Start()
    {
        UpdateText();
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
        stats.Health.OnValueChanged += UpdateText;
        stats.Poise.OnValueChanged += UpdateText;
    }

    private void OnDisable()
    {
        stats.Health.OnValueChanged -= UpdateText;
        stats.Poise.OnValueChanged -= UpdateText;
    }

    void UpdateText()
    {
        hpText.text = "HP: " + stats.Health.CurrentValue.ToString() +
            "\nST: " + stats.Poise.CurrentValue.ToString();
    }
}
