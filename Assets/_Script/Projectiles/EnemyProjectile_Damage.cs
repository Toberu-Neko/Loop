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
            damageable.Damage(details.combatDetails.damageAmount, transform.position);
        }
        if (collider.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.Knockback(details.combatDetails.knockbackAngle, details.combatDetails.knockbackStrength, transform.position);
        }
        if (collider.TryGetComponent(out IStaminaDamageable staminaDamageable))
        {
            staminaDamageable.TakeStaminaDamage(details.combatDetails.staminaDamageAmount, transform.position);
        }

        if (countered)
        {
            if (collider.TryGetComponent(out IMapDamageableItem mapDamageableItem))
            {
                mapDamageableItem.TakeDamage(details.combatDetails.damageAmount);
            }
        }

        ReturnToPool();
    }

    private void HandleHitGround()
    {
        ReturnToPool();
    }
}

