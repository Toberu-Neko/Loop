using TMPro;
using UnityEngine;

public class DebugEntityStats : WorldCanvasBase
{
    [SerializeField] private HealthBar hp;
    [SerializeField] private HealthBar st;
    private Core core;
    private Stats stats;

    private bool firstInit;

    protected override void Awake()
    {
        base.Awake();

        core = GetComponentInParent<Core>();
        stats = core.GetCoreComponent<Stats>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        stats.Health.OnValueChanged += UpdateBar;
        stats.Stamina.OnValueChanged += UpdateBar;

        firstInit = true;

        UpdateBar();
    }

    protected override void OnDisable()
    {
        stats.Health.OnValueChanged -= UpdateBar;
        stats.Stamina.OnValueChanged -= UpdateBar;
    }

    void UpdateBar()
    {
        if (firstInit)
        {
            firstInit = false;
            hp.Init(stats.Health.MaxValue);
            st.Init(stats.Stamina.MaxValue);
        }

        hp.UpdateHealthBar(stats.Health.CurrentValue);
        st.UpdateHealthBar(stats.Stamina.CurrentValue);
    }
}
