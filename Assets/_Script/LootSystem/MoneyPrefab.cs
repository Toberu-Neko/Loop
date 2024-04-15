using UnityEngine;

public class MoneyPrefab : DropableItemBase
{
    [SerializeField] private Sound pickSFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventoryManager.Instance.AddMoney(1);
            AudioManager.Instance.PlaySoundFX(pickSFX, transform, AudioManager.SoundType.twoD);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
