using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;


public class MultiBossFightUI : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent bossNameText1;
    [SerializeField] private HealthBar bossHealthBar1;
    [SerializeField] private HealthBar bossSTBar1;
    [SerializeField] private GameObject bossUI1;

    [SerializeField] private LocalizeStringEvent bossNameText2;
    [SerializeField] private HealthBar bossHealthBar2;
    [SerializeField] private HealthBar bossSTBar2;
    [SerializeField] private GameObject bossUI2;

    [SerializeField] private LocalizedString defaultBossName;
    private BossBase boss1;
    private BossBase boss2;

    public void Active(BossBase bossBase, BossBase bossBase2)
    {
        boss1 = bossBase;
        boss2 = bossBase2;

        bossUI1.SetActive(true);
        bossUI2.SetActive(true);

        gameObject.SetActive(true);
        bossHealthBar1.Init(boss1.Stats.Health.MaxValue);
        bossSTBar1.Init(boss1.Stats.Stamina.MaxValue);
        boss1.Stats.Health.OnValueChanged += UpdateHealthBar1; 
        boss1.Stats.Stamina.OnValueChanged += UpdateSTBar1;
        boss1.Stats.Health.OnCurrentValueZero += Deactive1;

        bossNameText1.StringReference = boss1.BossNameLocalized;

        bossHealthBar2.Init(boss2.Stats.Health.MaxValue);
        bossSTBar2.Init(boss2.Stats.Stamina.MaxValue);
        boss2.Stats.Health.OnValueChanged += UpdateHealthBar2;
        boss2.Stats.Stamina.OnValueChanged += UpdateSTBar2;
        boss2.Stats.Health.OnCurrentValueZero += Deactive2;

        bossNameText2.StringReference = boss2.BossNameLocalized;
    }

    private void Deactive1()
    {
        boss1.Stats.Health.OnValueChanged -= UpdateHealthBar1;
        boss1.Stats.Stamina.OnValueChanged -= UpdateSTBar1;
        boss1.Stats.Health.OnCurrentValueZero -= Deactive1;

        boss1 = null;
        bossHealthBar1.Deactivate();
        bossUI1.SetActive(false);
    }

    private void Deactive2()
    {
        boss2.Stats.Health.OnValueChanged -= UpdateHealthBar2;
        boss2.Stats.Stamina.OnValueChanged -= UpdateSTBar2;
        boss2.Stats.Health.OnCurrentValueZero -= Deactive2;

        boss2 = null;
        bossHealthBar2.Deactivate();
        bossUI2.SetActive(false);
    }

    public void Deactive()
    {
        Deactive1();
        Deactive2();
        gameObject.SetActive(false);
    }

    private void UpdateHealthBar1()
    {
        bossHealthBar1.UpdateHealthBar(boss1.Stats.Health.CurrentValue);
    }

    private void UpdateSTBar1()
    {
        bossSTBar1.UpdateHealthBar(boss1.Stats.Stamina.CurrentValue);
    }


    private void UpdateHealthBar2()
    {
        bossHealthBar2.UpdateHealthBar(boss2.Stats.Health.CurrentValue);
    }

    private void UpdateSTBar2()
    {
        bossSTBar2.UpdateHealthBar(boss2.Stats.Stamina.CurrentValue);
    }
}
