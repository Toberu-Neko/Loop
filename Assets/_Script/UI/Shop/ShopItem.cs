using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetValue(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
