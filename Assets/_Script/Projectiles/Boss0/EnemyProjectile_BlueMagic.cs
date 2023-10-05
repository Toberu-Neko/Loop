using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_BlueMagic : EnemyProjectileBase
{
    [SerializeField] private float startRadius = 1f;
    [SerializeField] private float expandRate = 1f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float damagePace = 0.33f;

    private float currentRadius;
    private bool startMagic;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHitTargetAction += HandleHitTarget;
        OnHitGroundAction += HandleHitGround;
        OnDuration += HandleHitGround;

        startMagic = false;
        currentRadius = startRadius;
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
            if (stats.IsTimeSlowed)
            {
                currentRadius += expandRate * Time.deltaTime * GameManager.Instance.TimeSlowMultiplier;
            }
            else if (!stats.IsTimeStopped)
            {
                currentRadius += expandRate * Time.deltaTime;
            }
        }
    }

    private void HandleHitTarget(Collider2D collider)
    {
        HandleHitGround();
    }

    private void HandleHitGround()
    {
        if (!startMagic)
        {
            startMagic = true;

            DoDamage();
        }
    }

    private void DoDamage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, currentRadius, Vector2.zero, 0f, _whatIsPlayer);

        foreach(var hit in hits)
        {
            hit.transform.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(details.damageAmount, transform.position, false);

            hit.transform.TryGetComponent(out IStaminaDamageable staminaDamageable);
            staminaDamageable?.TakeStaminaDamage(details.staminaDamageAmount, transform.position, false);
        }

        Invoke(nameof(DoDamage), damagePace);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        // collision.transform.TryGetComponent(out ISlowable slowable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
