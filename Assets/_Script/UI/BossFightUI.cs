using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;


public class BossFightUI : MonoBehaviour
{
    [SerializeField] private HealthBar bossHealthBar;
    [SerializeField] private HealthBar bossSTBar;

    [SerializeField] private LocalizeStringEvent bossNameText;
    [SerializeField] private LocalizedString defaultBossName;
    private BossBase boss;

    public void Active(BossBase bossBase)
    {
        boss = bossBase;
        gameObject.SetActive(true);
        bossHealthBar.Init(boss.Stats.Health.MaxValue);
        bossSTBar.Init(boss.Stats.Stamina.MaxValue);
        boss.Stats.Health.OnValueChanged += UpdateHealthBar; 
        boss.Stats.Health.OnCurrentValueZero += Deactive;

        boss.Stats.Stamina.OnValueChanged += UpdateSTBar;

        bossNameText.StringReference = boss.BossNameLocalized;
    }

    public void Deactive()
    {
        boss.Stats.Health.OnValueChanged -= UpdateHealthBar;
        boss.Stats.Health.OnCurrentValueZero -= Deactive;

        boss.Stats.Stamina.OnValueChanged -= UpdateSTBar;

        boss = null;
        bossHealthBar.Deactivate();
        gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        bossHealthBar.UpdateHealthBar(boss.Stats.Health.CurrentValue);
    }

    private void UpdateSTBar()
    {
        bossSTBar.UpdateHealthBar(boss.Stats.Stamina.CurrentValue);
    }
}
