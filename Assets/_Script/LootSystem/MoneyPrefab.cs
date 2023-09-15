using UnityEngine;

public class MoneyPrefab : DropableItemBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventoryManager.Instance.AddMoney(1);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
