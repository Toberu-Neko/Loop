using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private float radius = 3f;
    private ProjectileDetails details;

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

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.name);

            if (hit.TryGetComponent(out IDamageable dam))
            {
                dam.Damage(details.damageAmount, transform.position);
            }

            if (hit.TryGetComponent(out IKnockbackable knock))
            {
                int direction = transform.position.x < hit.transform.position.x ? 1 : -1;
                knock.Knockback(details.knockbackAngle, details.knockbackStrength, direction, transform.position);
            }

            if (hit.TryGetComponent(out IStaminaDamageable damStamina))
            {
                damStamina.TakeStaminaDamage(details.staminaDamageAmount, transform.position);
            }

            if (hit.TryGetComponent(out IMapDamageableItem mapItem))
            {
                mapItem.TakeDamage(details.damageAmount);
            }

        }

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
