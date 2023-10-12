using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Rewind : EnemyProjectile_Base, IRewindable
{
    private bool startRewind = false;
    private bool hitPlayer = false;
    private bool fire = false;

    public override void Fire(Vector2 fireDirection, ProjectileDetails details)
    {
        base.Fire(fireDirection, details);

        fire = true;
    }

    public override void HandlePerfectBlock()
    {
        base.HandlePerfectBlock();
    }

    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true, bool forceKnockback = false)
    {
        ReturnToPool();
    }

    public void Rewind(bool doRewind = true)
    {
        if (doRewind)
        {
            startRewind = true;
        }
        else
        {
            ReturnToPool();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnHitTargetAction -= HandleHitTarget;
        OnHitGroundAction -= HandleHitGround;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        hitPlayer = false;
        startRewind = false;
        fire = false;

        OnHitTargetAction += HandleHitTarget;
        OnHitGroundAction += HandleHitGround;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void Update()
    {
        core.LogicUpdate();
        startTime = stats.Timer(startTime);

        if (startRewind)
        {
            movement.SetVelocity(details.speed * -fireDirection);

            if (Vector2.Distance((Vector2)transform.position, startPos) < 0.1f)
            {
                ReturnToPool();
            }
        }
        else if (!hasHitGround && fire)
        {
            movement.SetVelocity(details.speed, fireDirection);
        }
        else
        {
            movement.SetVelocityZero();
        }
    }


    private void HandleHitTarget(Collider2D collider)
    {
        if (hitPlayer)
            return;

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
    }

    private void HandleHitGround()
    {
        hitPlayer = false;
    }
}
