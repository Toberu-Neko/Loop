using UnityEngine;

public class EnemyProjectile_Damage : EnemyProjectileBase
{
    [SerializeField] private float damageRadius;
    [SerializeField] private Transform damagePosition;
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rig;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnAction += HandleAction;
    }

    protected override void OnDisable()
    {
        base.OnEnable();

        OnAction -= HandleAction;
    }

    private void HandleAction(Collider2D collider)
    {

        if (collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(details.damageAmount, transform.position);
        }
        if (collider.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.Knockback(details.knockbackAngle, details.knockbackStrength, transform.position);
        }
        if (collider.TryGetComponent(out IStaminaDamageable staminaDamageable))
        {
            staminaDamageable.TakeStaminaDamage(details.staminaDamageAmount, transform.position);
        }

        if (countered)
        {
            if (collider.TryGetComponent(out IMapDamageableItem mapDamageableItem))
            {
                mapDamageableItem.TakeDamage(details.damageAmount);
            }
        }

        ReturnToPool();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }


}

