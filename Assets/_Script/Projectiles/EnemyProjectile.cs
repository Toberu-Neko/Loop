using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IKnockbackable
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    private LayerMask _whatIsPlayer;
    [SerializeField] private float damageRadius;
    [SerializeField] private Transform damagePosition;
    [SerializeField] private Collider2D col;
    [SerializeField] private Core core;
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private Animator anim;
    private Vector2 fireDirection;

    private float travelDistance;
    private float xStartPosition;
    private int facingDirection;
    private bool hasHitGround;
    private bool countered;
    private bool damaged = false;
    private Vector2 counterVelocity;


    private ProjectileDetails details;
    private Movement movement;
    private Stats stats;

    private void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();
    }


    private void Update()
    {
        core.LogicUpdate();

        if (!hasHitGround && !countered)
        {
            movement.SetVelocity(details.speed, fireDirection);
        }
        if (!hasHitGround && countered)
        {
            movement.SetVelocity(counterVelocity);
        }
        if (hasHitGround)
        {
            movement.SetVelocityZero();
        }
    }

    private void LateUpdate()
    {
        core.LateLogicUpdate();
    }

    private void FixedUpdate()
    {
        core.PhysicsUpdate();
    }

    private void OnEnable()
    {
        hasHitGround = false;
        damaged = false;
        countered = false;

        stats.OnTimeSlowStart += HandleChangeAnimSlow;
        stats.OnTimeSlowEnd += HandleChangeAnimOrigin;
        stats.OnTimeStopStart += HandleChangeAnimSlow;
        stats.OnTimeStopEnd += HandleChangeAnimOrigin;
    }

    private void OnDisable()
    {
        anim.SetBool("timeSlow", false);

        stats.OnTimeStopStart -= HandleChangeAnimSlow;
        stats.OnTimeStopEnd -= HandleChangeAnimOrigin;
        stats.OnTimeSlowStart -= HandleChangeAnimSlow;
        stats.OnTimeSlowEnd -= HandleChangeAnimOrigin;
    }

    private void HandleChangeAnimSlow()
    {
        anim.SetBool("timeSlow", true);
    }

    private void HandleChangeAnimOrigin()
    {
        anim.SetBool("timeSlow", false);
    }

    public void FireProjectile(ProjectileDetails details, int facingDirection, Vector2 fireDirection)
    {
        this.details = details;
        this.facingDirection = facingDirection;
        this.fireDirection = fireDirection;


        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, fireDirection);

        transform.rotation = targetRotation; 
        movement.SetGravityZero();
        movement.SetVelocity(details.speed, fireDirection);

        xStartPosition = transform.position.x;

        gameObject.layer = LayerMask.NameToLayer("EnemyAttack");
        _whatIsPlayer = whatIsPlayer;
        Invoke(nameof(ReturnToPool), 10f);
    }

    private void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));
        if (!stats.InCombat || damaged || hasHitGround)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
        else
        {
            Invoke(nameof(ReturnToPool), 3f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }

    public void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true, bool forceKnockback = false)
    {
        if ((stats.IsTimeStopped || stats.IsTimeSlowed) && !countered)
        {
            countered = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
            _whatIsPlayer = LayerMask.GetMask("Damageable");
            xStartPosition = transform.position.x;

            int direction = transform.position.x < damagePosition.x ? -1 : 1;

            if (stats.IsTimeStopped)
            {
                if (facingDirection != direction)
                {
                    counterVelocity = movement.TimeStopVelocity * -4f;
                    movement.SetTimeStopVelocity(counterVelocity);
                    facingDirection = direction;
                    movement.Turn();
                }
                else
                {
                    movement.SetTimeStopVelocity(counterVelocity);
                }
            }

            else if (stats.IsTimeSlowed)
            {
                if (facingDirection != direction)
                {
                    counterVelocity = -movement.CurrentVelocity / GameManager.Instance.TimeSlowMultiplier;
                    movement.SetTimeSlowVelocity(counterVelocity / GameManager.Instance.TimeSlowMultiplier * 5f);
                    facingDirection = direction;
                    movement.Turn();
                }
                else
                {
                    counterVelocity = movement.TimeSlowVelocity * 4f;
                    movement.SetVelocity(movement.CurrentVelocity);
                    movement.SetTimeSlowVelocity(counterVelocity);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _whatIsPlayer) != 0 && !hasHitGround && !damaged)
        {
            damaged = true;
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(details.damageAmount, transform.position);
            }
            if (collision.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(details.knockbackAngle, details.knockbackStrength, transform.position);
            }
            if (collision.TryGetComponent(out IStaminaDamageable staminaDamageable))
            {
                staminaDamageable.TakeStaminaDamage(details.staminaDamageAmount, transform.position);
            }

            if (countered)
            {
                if (collision.TryGetComponent(out IMapDamageableItem mapDamageableItem))
                {
                    mapDamageableItem.TakeDamage(details.damageAmount);
                }
            }

            ReturnToPool();
        }

        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            hasHitGround = true;
            movement.SetVelocityZero();

            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), 5f);
        }
        
    } 

}

