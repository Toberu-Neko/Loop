using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Slider easeSlider;

    [SerializeField] private float easeSpeed = 0.05f;
    private float maxHealth;
    private float currentHealth;

    public void Init(float maxValue)
    {
        gameObject.SetActive(true);
        maxHealth = maxValue;
        healthBarSlider.maxValue = maxHealth;
        easeSlider.maxValue = maxHealth;
        easeSlider.value = maxHealth;
        UpdateHealthBar(maxHealth);
    }

    public void UpdateHealthBar(float currentValue)
    {
        currentHealth = currentValue;
        healthBarSlider.value = currentHealth;

        Invoke(nameof(UpdateEaseBar), Time.deltaTime);
    }

    private void UpdateEaseBar()
    {
        if (healthBarSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, healthBarSlider.value, easeSpeed);
            Invoke(nameof(UpdateEaseBar), Time.deltaTime);
        }
    }

    public void Deactivate()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }
    
}
