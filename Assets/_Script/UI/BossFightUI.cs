using TMPro;
using UnityEngine;


public class BossFightUI : MonoBehaviour
{
    [SerializeField] private HealthBar bossHealthBar;
    [SerializeField] private HealthBar bossSTBar;
    [SerializeField] private TextMeshProUGUI bossNameText;
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

        bossNameText.text = boss.BossName;
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
