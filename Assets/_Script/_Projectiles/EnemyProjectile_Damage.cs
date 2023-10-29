using UnityEngine;
using System;

public class EnemyProjectile_Damage : EnemyProjectile_Base
{
    public event Action OnHitPlayer;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHitGroundAction += HandleHitGround;
        OnDuration += ReturnToPool;
        OnHitTargetAction += HandleHitTarget;
    }

    protected override void OnDisable()
    {
        base.OnEnable();

        OnHitGroundAction -= HandleHitGround;
        OnDuration -= ReturnToPool;
        OnHitTargetAction -= HandleHitTarget;
    }

    private void HandleHitTarget(Collider2D collider)
    {
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

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if(collider.CompareTag("Player"))
        {
            OnHitPlayer?.Invoke();
        }
    }

    private void HandleHitGround()
    {
        ReturnToPool();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Fire(Vector2 fireDirection, float speed, ProjectileDetails details)
    {
        base.Fire(fireDirection, speed, details);
    }

    public override void HandlePerfectBlock()
    {
        base.HandlePerfectBlock();
    }

    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        base.Knockback(angle, force, damagePosition, blockable);
    }

    protected override void ReturnToPool()
    {
        base.ReturnToPool();
    }
}

