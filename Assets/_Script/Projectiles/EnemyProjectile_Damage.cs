using UnityEngine;

public class EnemyProjectile_Damage : EnemyProjectile_Base
{

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHitGroundAction += HandleHitGround;
        OnDuration += ReturnToPool;
    }

    protected override void OnDisable()
    {
        base.OnEnable();

        OnHitGroundAction -= HandleHitGround;
        OnDuration -= ReturnToPool;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

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

    private void HandleHitGround()
    {
        ReturnToPool();
    }
}

