using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private bool destoryWhenCollide;
    private List<GameObject> collidedObjects = new();

    private ProjectileDetails projectileDetails;

    private Vector2 fireDirection;
    private int knockbackDirection;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.gravityScale = 0;
        rb.velocity = fireDirection * projectileDetails.speed;

        Invoke(nameof(DestoryThis), projectileDetails.duration);
    }
    public void Fire(ProjectileDetails details, Vector2 fireDirection)
    {
        projectileDetails = details;
        this.fireDirection = fireDirection;

        if(fireDirection.x > 0)
        {
            knockbackDirection = 1;
        }
        else
        {
            knockbackDirection = -1;
        }
    }

    private void DestoryThis()
    {
        CancelInvoke(nameof(DestoryThis));
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.gameObject.name);
        if(!collision.gameObject.CompareTag("Player") && !collidedObjects.Contains(collision.gameObject))
        {
            if(collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(projectileDetails.damageAmount, transform.position, true);
            }
            if(collision.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(projectileDetails.knockbackAngle, projectileDetails.knockbackStrength, knockbackDirection, transform.position);
            }
            if(collision.TryGetComponent(out IStaminaDamageable staminaDamageable))
            {
                staminaDamageable.TakeStaminaDamage(projectileDetails.staminaDamageAmount, transform.position, true);
            }

            collidedObjects.Add(collision.gameObject);

            if (destoryWhenCollide)
                DestoryThis();
        }

    }
}
