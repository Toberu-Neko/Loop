using UnityEngine;

public class GunChargeAttackScript : MonoBehaviour
{
    [SerializeField] private GameObject chargeAttackObj;
    [SerializeField] private BoxCollider2D chargeAttackCollider;
    [SerializeField] private SpriteRenderer chargeSpriteRenderer;

    [SerializeField] private Transform endSpriteObj;
    [SerializeField] private SpriteRenderer endSpriteRenderer;

    public void Init(Vector2 size, Vector3 angle)
    {
        chargeAttackCollider.size = size;
        chargeSpriteRenderer.size = size;
        chargeAttackObj.transform.position = new(transform.position.x + size.x / 2, transform.position.y, transform.position.z);

        endSpriteObj.position = new(transform.position.x + size.x - endSpriteRenderer.size.x / 2, transform.position.y, transform.position.z);
        transform.localEulerAngles = angle;
    }
}
