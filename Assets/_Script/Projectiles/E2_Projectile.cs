using UnityEngine;

public class E2_Projectile : MonoBehaviour, IKnockbackable
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
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

    private ProjectileDetails details;
    private Movement movement;
    private Stats stats;

    private void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();

        countered = false;
        damaged = false;
    }
    private void Start()
    {
        movement.SetGravityZero();
        movement.SetVelocity(details.speed, transform.right);

        isGravityOn = false;
        xStartPosition = transform.position.x;
    }
    private void Update()
    {
        core.LogicUpdate();

        if (!hasHitGround)
        {
            if (isGravityOn && !stats.IsTimeStopped)
            {
                float angle = Mathf.Atan2(movement.CurrentVelocity.y, movement.CurrentVelocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

    }

    private void FixedUpdate()
    {

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

            if (Mathf.Abs(xStartPosition - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                movement.SetGravityOrginal();
            }
        }
    }

    public void FireProjectile(ProjectileDetails details, float travelDistance, int facingDirection)
    {
        this.details = details;
        this.travelDistance = travelDistance;
        this.facingDirection = facingDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }

    public void Knockback(Vector2 angle, float force, int direction, Vector2 damagePosition, bool blockable = true)
    {
        if (stats.IsTimeStopped && !countered)
        {
            countered = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
            whatIsPlayer = LayerMask.GetMask("Damageable");
            xStartPosition = transform.position.x;

            if (facingDirection != direction)
            {
                movement.SetTimeStopVelocity(movement.TimeStopVelocity * -4f);
                facingDirection = direction;
                // movement.Turn();
            }
            else
            {
                movement.SetTimeStopVelocity(movement.TimeStopVelocity * 4f);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsPlayer) != 0 && !hasHitGround && !damaged)
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

            Destroy(gameObject);
        }

        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            hasHitGround = true;
            movement.SetGravityZero();
            movement.SetVelocityZero();

            Destroy(gameObject, 5f);
        }
        
    } 
}

