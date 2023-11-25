using UnityEngine;

public class EP_BlueStatic : EP_StaticBase
{
    [Header("Blue Magic")]
    [SerializeField] private BlueMagicVariables variables;

    [SerializeField] private GameObject sphereObj;
    private Vector3 sphereOrgScale;

    private float currentRadius = 1f;
    private float startMagicTime;
    private float lastDamageTime;

    protected override void Awake()
    {
        base.Awake();
        sphereOrgScale = sphereObj.transform.localScale;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        sphereObj.SetActive(false);
        currentRadius = variables.startRadius;
        lastDamageTime = 0f;
        startMagicTime = 0f;

        SR.enabled = true;
        OnExplodeAction += HandleExplodeAction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnExplodeAction -= HandleExplodeAction;
    }

    protected override void Update()
    {
        base.Update();

        if (state == State.Explode)
        {
            startMagicTime = stats.Timer(startMagicTime);
            lastDamageTime = stats.Timer(lastDamageTime);

            ExpandRadius();

            if (Time.time >= lastDamageTime + variables.damagePace)
            {
                DoDamage();
            }

            if (Time.time >= startMagicTime + variables.duration)
            {
                ReturnToPool();
            }
        }
    }
    private void HandleExplodeAction()
    {
        sphereObj.SetActive(true);
        lastDamageTime = 0f;
        startMagicTime = Time.time;
    }

    private void ExpandRadius()
    {
        if (stats.IsTimeSlowed)
        {
            currentRadius += variables.expandRate * Time.deltaTime * GameManager.Instance.TimeSlowMultiplier;
        }
        else if (!stats.IsTimeStopped)
        {
            currentRadius += variables.expandRate * Time.deltaTime;
        }

        sphereObj.transform.localScale = sphereOrgScale * currentRadius;
    }
    private void DoDamage()
    {
        lastDamageTime = Time.time;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, currentRadius, whatIsTargetLayer);

        foreach (var col in cols)
        {
            col.transform.TryGetComponent(out IDamageable damageable);
            damageable?.Damage(details.combatDetails.damageAmount, transform.position, false);

            col.transform.TryGetComponent(out IStaminaDamageable staminaDamageable);
            staminaDamageable?.TakeStaminaDamage(details.combatDetails.staminaDamageAmount, transform.position, false);

            col.transform.TryGetComponent(out ISlowable slowable);
            slowable?.SetDebuffMultiplier(variables.slowMultiplier, variables.damagePace);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        if (state == State.Moving)
        {
            base.Knockback(angle, force, damagePosition, blockable);
        }
    }
    public override void Init(Vector2 destination, float explodeTime)
    {
        base.Init(destination, explodeTime);
    }

}
