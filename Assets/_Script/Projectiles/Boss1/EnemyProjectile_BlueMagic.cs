using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_BlueMagic : EnemyProjectile_Base
{
    [SerializeField] private float startRadius = 1f;
    [SerializeField] private float expandRate = 1f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float damagePace = 0.33f;
    [SerializeField] private float slowMultiplier = 0.5f;
    
    [SerializeField] private GameObject sphereObj;
    private Vector3 sphereOrgScale;

    private float currentRadius = 1f;
    private float startMagicTime;
    private bool startMagic;
    private float lastDamageTime;

    protected override void Awake()
    {
        base.Awake();

        sphereOrgScale = sphereObj.transform.localScale;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHitTargetAction += HandleHitTarget;
        OnHitGroundAction += HandleHitGround;
        OnDuration += HandleHitGround;

        startMagic = false;
        sphereObj.SetActive(false);
        currentRadius = startRadius;
        lastDamageTime = 0f;
        startMagicTime = 0f;

        SR.enabled = true;
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
            startMagicTime = stats.Timer(startMagicTime);
            lastDamageTime = stats.Timer(lastDamageTime);

            if (stats.IsTimeSlowed)
            {
                currentRadius += expandRate * Time.deltaTime * GameManager.Instance.TimeSlowMultiplier;
            }
            else if (!stats.IsTimeStopped)
            {
                currentRadius += expandRate * Time.deltaTime;
            }

            sphereObj.transform.localScale = sphereOrgScale  * currentRadius;

            if(Time.time >= lastDamageTime + damagePace)
            {
                DoDamage();
            }

            if(Time.time >= startMagicTime + duration)
            {
                ReturnToPool();
            }
        }
    }

    public override void Fire(Vector2 fireDirection, ProjectileDetails details)
    {
        base.Fire(fireDirection, details);
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
            SR.enabled = false;
            startMagic = true;
            startMagicTime = Time.time;

            sphereObj.SetActive(true);
        }
    }

    private void DoDamage()
    {
        lastDamageTime = Time.time;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, currentRadius, whatIsTargetLayer);

        foreach (var col in cols)
        {
            col.transform.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(details.damageAmount, transform.position, false);

            col.transform.TryGetComponent(out IStaminaDamageable staminaDamageable);
            staminaDamageable?.TakeStaminaDamage(details.staminaDamageAmount, transform.position, false);

            col.transform.TryGetComponent(out ISlowable slowable);
            slowable?.SetActionSpeedMultiplier(slowMultiplier, damagePace);
        }
    }
    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        base.Knockback(angle, force, damagePosition, blockable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
