using UnityEngine;

public class EP_RedStatic : EP_StaticBase
{
    [SerializeField] private float startRadius = 5f;

    [SerializeField] private GameObject particleObj;


    protected override void OnEnable()
    {
        base.OnEnable();

        OnExplodeAction += HandleExplode;

        particleObj.SetActive(false);

        SR.enabled = true;
    }


    private void HandleExplode()
    {
        particleObj.SetActive(true);
        SR.enabled = false;
        DoDamage();
        Invoke(nameof(ReturnToPool), 0.5f);
    }

    private void DoDamage()
    {
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

    protected override void OnDisable()
    {
        base.OnDisable();

        OnExplodeAction -= HandleExplode;
    }


    public override void HandlePerfectBlock()
    {
        base.HandlePerfectBlock();
    }

    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        if (state == State.Moving)
        {
            base.Knockback(angle, force, damagePosition, blockable);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
    }

    protected override void ReturnToPool()
    {
        base.ReturnToPool();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Init(Vector2 destination, float explodeTime)
    {
        base.Init(destination, explodeTime);
    }
}
