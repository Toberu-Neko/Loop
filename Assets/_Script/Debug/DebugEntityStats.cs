using TMPro;
using UnityEngine;

public class DebugEntityStats : WorldCanvasBase
{
    [SerializeField] TextMeshProUGUI hpText;
    private Core core;
    private Stats stats;

    protected override void Awake()
    {
        base.Awake();

        core = GetComponentInParent<Core>();
        stats = core.GetCoreComponent<Stats>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        stats.Health.OnValueChanged += UpdateText;
        stats.Stamina.OnValueChanged += UpdateText;

        UpdateText();
    }

    protected override void OnDisable()
    {
        stats.Health.OnValueChanged -= UpdateText;
        stats.Stamina.OnValueChanged -= UpdateText;
    }

    void UpdateText()
    {
        hpText.text = "HP: " + stats.Health.CurrentValue.ToString() +
            "\nST: " + stats.Stamina.CurrentValue.ToString();
    }
}
