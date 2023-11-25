using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_RedMagic : EnemyProjectile_Base
{
    [SerializeField] private float startRadius = 2f;
    [SerializeField] private float duration = 0.5f;

    [SerializeField] private GameObject sphereObj;

    private float startMagicTime;
    private bool startMagic;
    private bool damaged;
    protected override void OnEnable()
    {
        base.OnEnable();

        OnHitTargetAction += HandleHitTarget;
        OnHitGroundAction += HandleHitGround;
        OnDuration += HandleHitGround;

        sphereObj.SetActive(false);

        SR.enabled = true;
        damaged = false;
        startMagic = false;
        startMagicTime = 0f;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

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

            if (Time.time >= startMagicTime + duration && !damaged)
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
            damageable.Damage(0f, transform.position, true);
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
        damaged = true;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, startRadius, whatIsTargetLayer);
        SR.enabled = false;

        foreach (var col in cols)
        {
            col.transform.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(details.combatDetails.damageAmount, transform.position, false);

            col.transform.TryGetComponent(out IStaminaDamageable staminaDamageable);
            staminaDamageable?.TakeStaminaDamage(details.combatDetails.staminaDamageAmount, transform.position, false);

            col.transform.TryGetComponent(out IKnockbackable knockbackable);
            knockbackable?.Knockback(details.combatDetails.knockbackAngle, details.combatDetails.knockbackStrength, transform.position, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, startRadius);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Fire(Vector2 fireDir)
    {
        base.Fire(fireDir);
    }

    public override void HandlePerfectBlock()
    {
        base.HandlePerfectBlock();
    }

    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        base.Knockback(angle, force, damagePosition, blockable);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
