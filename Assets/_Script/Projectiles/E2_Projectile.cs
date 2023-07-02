using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_Projectile : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private float travelDistance;
    private float xStartPosition;
    private int facingDirection;

    [SerializeField] private float gravity;
    [SerializeField] private float damageRadius;

    private bool isGravityOn;
    private bool hasHitGround;

    private Rigidbody2D rb;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform damagePosition;

    ProjectileDetails details;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.gravityScale = 0f;
        rb.velocity = transform.right * details.speed;

        isGravityOn = false;

        xStartPosition = transform.position.x;
    }
    private void Update()
    {
        if (!hasHitGround)
        {
            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                if(damageHit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(details.damageAmount, transform.position);
                }
                if(damageHit.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.Knockback(details.knockbackAngle, details.knockbackStrength, facingDirection, transform.position);
                }
                if(damageHit.TryGetComponent(out IStaminaDamageable staminaDamageable))
                {
                    staminaDamageable.TakeStaminaDamage(details.staminaDamageAmount, transform.position);
                }
                
                Destroy(gameObject);
            }

            if (groundHit)
            {
                hasHitGround = true;
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;

                Destroy(gameObject, 5f);

            }

            if (Mathf.Abs(xStartPosition - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }
    }

    public void FireProjectile(ProjectileDetails details, float travelDistance, int facingDirection)
    {
        this.details = details;
        this.travelDistance = travelDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
