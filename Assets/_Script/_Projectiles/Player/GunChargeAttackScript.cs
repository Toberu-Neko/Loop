using UnityEngine;

public class GunChargeAttackScript : MonoBehaviour
{
    [SerializeField] private GameObject chargeAttackObj;
    [SerializeField] private BoxCollider2D chargeAttackCollider;
    [SerializeField] private SpriteRenderer chargeSpriteRenderer;

    [SerializeField] private Transform endSpriteObj;
    [SerializeField] private SpriteRenderer endSpriteRenderer;

    private Vector3 workspace;
    public void Init(Vector2 size)
    {
        chargeAttackCollider.size = size;
        chargeSpriteRenderer.size = size;

        workspace.Set(transform.localPosition.x + size.x / 2, transform.localPosition.y, transform.localPosition.z);
        chargeAttackObj.transform.localPosition = workspace;

        workspace.Set(transform.localPosition.x + size.x - endSpriteRenderer.size.x / 2, transform.localPosition.y, transform.localPosition.z);
        endSpriteObj.localPosition = workspace;
    }
}
