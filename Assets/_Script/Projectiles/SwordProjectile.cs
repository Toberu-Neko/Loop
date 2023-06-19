using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordProjectile : MonoBehaviour
{
    private List<GameObject> collidedObjects = new();

    private float speed;
    private float duration;
    private int facingDirection;

    private float damageAmount = 10;
    private Vector2 knockbackAngle = new(2, 1);
    private float knockbackStrength = 10;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.gravityScale = 0;
        rb.velocity = transform.right * speed;

        Invoke(nameof(DestoryThis), duration);
    }
    public void Fire(float damageAmount, float speed, float duration, int facingDirection, Vector2 knockbackAngle, float knockbackStrength)
    {
        this.speed = speed;
        this.duration = duration;
        this.facingDirection = facingDirection;
        this.damageAmount = damageAmount;
            
        this.knockbackAngle = knockbackAngle;
        this.knockbackStrength = knockbackStrength;
    }

    private void DestoryThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player") && !collidedObjects.Contains(collision.gameObject))
        {
            if(collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(damageAmount, transform.position, true);
            }
            if(collision.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(knockbackAngle, knockbackStrength, facingDirection, transform.position);
            }
            collidedObjects.Add(collision.gameObject);
        }

    }
}
