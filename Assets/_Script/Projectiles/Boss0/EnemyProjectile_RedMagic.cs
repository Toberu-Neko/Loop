using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_RedMagic : EnemyProjectileBase
{
    [SerializeField] private float startRadius = 2f;
    [SerializeField] private float duration = 0.5f;

    [SerializeField] private GameObject sphereObj;

    private float startMagicTime;
    private bool startMagic;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHitTargetAction += HandleHitTarget;
        OnHitGroundAction += HandleHitGround;
        OnDuration += HandleHitGround;

        sphereObj.SetActive(false);

        SR.enabled = true;
        startMagic = false;
        startMagicTime = 0f;
    }

    protected override void OnDisable()
    {
        base.OnEnable();

        OnHitTargetAction -= HandleHitTarget;
        OnHitGroundAction -= HandleHitGround;
        OnDuration -= HandleHitGround;
    }

    protected override void Update()
    {
        base.Update();

        if (startMagic)
        {
            startMagicTime = Timer(startMagicTime);

            if (Time.time >= startMagicTime + duration)
            {
                DoDamage();
                ReturnToPool();
            }
        }
    }

    private void HandleHitTarget(Collider2D collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(0.1f, transform.position, true);
        }
        HandleHitGround();
    }

    private void HandleHitGround()
    {
        if (!startMagic)
        {
            startMagic = true;
            startMagicTime = Time.time;

            sphereObj.SetActive(true);
        }
    }

    private void DoDamage()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, startRadius, whatIsTargetLayer);
        SR.enabled = false;

        foreach (var col in cols)
        {
            col.transform.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(details.damageAmount, transform.position, false);

            col.transform.TryGetComponent(out IStaminaDamageable staminaDamageable);
            staminaDamageable?.TakeStaminaDamage(details.staminaDamageAmount, transform.position, false);

            col.transform.TryGetComponent(out IKnockbackable knockbackable);
            knockbackable?.Knockback(transform.position, details.knockbackStrength, details.knockbackAngle, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, startRadius);
    }
}
