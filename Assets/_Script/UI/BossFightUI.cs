using TMPro;
using UnityEngine;


public class BossFightUI : MonoBehaviour
{
    [SerializeField] private HealthBar bossHealthBar;
    [SerializeField] private TextMeshProUGUI bossNameText;
    private BossBase boss;

    public void Active(BossBase bossBase)
    {
        boss = bossBase;
        gameObject.SetActive(true);
        bossHealthBar.Init(boss.Stats.Health.MaxValue);
        boss.Stats.Health.OnValueChanged += UpdateHealthBar; 
        boss.Stats.Health.OnCurrentValueZero += Deactive;
        bossNameText.text = boss.BossName;
    }

    public void Deactive()
    {
        boss.Stats.Health.OnValueChanged -= UpdateHealthBar;
        boss.Stats.Health.OnCurrentValueZero -= Deactive;
        boss = null;
        bossHealthBar.Deactivate();
        gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        bossHealthBar.UpdateHealthBar(boss.Stats.Health.CurrentValue);
    }
}
