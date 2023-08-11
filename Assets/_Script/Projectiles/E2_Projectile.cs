using UnityEngine;

public class E2_Projectile : MonoBehaviour, IKnockbackable
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    private LayerMask _whatIsPlayer;
    [SerializeField] private float damageRadius;
    [SerializeField] private Transform damagePosition;
    [SerializeField] private Collider2D col;
    [SerializeField] private Core core;

    private float travelDistance;
    private float xStartPosition;
    private int facingDirection;
    private bool isGravityOn;
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
            movement.SetVelocity(details.speed, transform.right);
        }
        if (!hasHitGround && countered)
        {
            movement.SetVelocity(counterVelocity);
        }
        /*
        if (!hasHitGround)
        {
            if (isGravityOn && !stats.IsTimeStopped)
            {
                float angle = Mathf.Atan2(movement.CurrentVelocity.y, movement.CurrentVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            movement.SetVelocity(details.speed, transform.right);
        }*/

    }

    private void FixedUpdate()
    {
        core.PhysicsUpdate();

        if (!hasHitGround)
        {
            /* Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                if (damageHit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(details.damageAmount, transform.position);
                }
                if (damageHit.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.Knockback(details.knockbackAngle, details.knockbackStrength, facingDirection, transform.position);
                }
                if (damageHit.TryGetComponent(out IStaminaDamageable staminaDamageable))
                {
                    staminaDamageable.TakeStaminaDamage(details.staminaDamageAmount, transform.position);
                }

                Destroy(gameObject);
            }

           if (groundHit)
            {
                hasHitGround = true;
                movement.SetGravityZero();
                movement.SetVelocityZero();

                Destroy(gameObject, 5f);
            }*/
            /*
            if (Mathf.Abs(xStartPosition - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                movement.SetGravityOrginal();
            }*/
        }
    }

    public void FireProjectile(ProjectileDetails details, float travelDistance, int facingDirection)
    {
        this.details = details;
        this.travelDistance = travelDistance;
        this.facingDirection = facingDirection;

        movement.SetGravityZero();
        movement.SetVelocity(details.speed, transform.right);

        isGravityOn = false;
        hasHitGround = false;
        damaged = false;
        countered = false;
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

    public void Knockback(Vector2 angle, float force, int direction, Vector2 damagePosition, bool blockable = true, bool forceKnockback = false)
    {
        if ((stats.IsTimeStopped || stats.IsTimeSlowed) && !countered)
        {
            countered = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
            _whatIsPlayer = LayerMask.GetMask("Damageable");
            xStartPosition = transform.position.x;

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
                    counterVelocity = movement.CurrentVelocity * -4f;
                    movement.SetVelocity(counterVelocity);
                    facingDirection = direction;
                    movement.Turn();
                }
                else
                {
                    counterVelocity = movement.CurrentVelocity * 4f;
                    movement.SetTimeStopVelocity(counterVelocity);
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
                knockbackable.Knockback(details.knockbackAngle, details.knockbackStrength, facingDirection, transform.position);
            }
            if (collision.TryGetComponent(out IStaminaDamageable staminaDamageable))
            {
                staminaDamageable.TakeStaminaDamage(details.staminaDamageAmount, transform.position);
            }

            ReturnToPool();
        }

        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            hasHitGround = true;
            movement.SetGravityZero();
            movement.SetVelocityZero();

            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), 10f);
        }
        
    } 
}

