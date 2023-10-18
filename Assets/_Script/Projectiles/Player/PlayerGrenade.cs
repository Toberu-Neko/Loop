using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private float radius = 3f;
    [SerializeField] private GameObject explodeObj;
    private ProjectileDetails details;

    private void OnEnable()
    {
        explodeObj.SetActive(false);
    }
    public void Throw(ProjectileDetails details, int direction)
    {
        if(direction == 1)
        {
            rig.velocity = details.speed * Vector2.right;
        }
        else
        {
            rig.velocity = details.speed * Vector2.left;
        }

        this.details = details;

        Invoke(nameof(Explode), details.duration);
    }

    private void Explode()
    {
        CancelInvoke(nameof(Explode));
        explodeObj.SetActive(true);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable dam))
            {
                dam.Damage(details.combatDetails.damageAmount, transform.position);
            }

            if (hit.TryGetComponent(out IKnockbackable knock))
            {
                knock.Knockback(details.combatDetails.knockbackAngle, details.combatDetails.knockbackStrength, transform.position);
            }

            if (hit.TryGetComponent(out IStaminaDamageable damStamina))
            {
                damStamina.TakeStaminaDamage(details.combatDetails.staminaDamageAmount, transform.position);
            }

            if (hit.TryGetComponent(out IMapDamageableItem mapItem))
            {
                mapItem.TakeDamage(details.combatDetails.damageAmount);
            }

        }


        Invoke(nameof(DestoryThis), 0.25f);
    }

    private void DestoryThis()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
