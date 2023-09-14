using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    private void Start()
    {
        PlayerInventoryManager.Instance.OnMoneyChanged += HandleMoneyUpdate;
        HandleMoneyUpdate();
    }

    private void OnDisable()
    {
        PlayerInventoryManager.Instance.OnMoneyChanged -= HandleMoneyUpdate;
    }

    private void HandleMoneyUpdate()
    {
        moneyText.text = "$: " + PlayerInventoryManager.Instance.Money.ToString();
    }
}
