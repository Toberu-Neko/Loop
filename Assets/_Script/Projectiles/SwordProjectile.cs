using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordProjectile : MonoBehaviour
{
    private List<GameObject> collidedObjects = new();

    private ProjectileDetails projectileDetails;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.gravityScale = 0;
        rb.velocity = transform.right * projectileDetails.speed;

        Invoke(nameof(DestoryThis), projectileDetails.duration);
    }
    public void Fire(ProjectileDetails details)
    {
        projectileDetails = details;
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
                damageable.Damage(projectileDetails.damageAmount, transform.position, true);
            }
            if(collision.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(projectileDetails.knockbackAngle, projectileDetails.knockbackStrength, projectileDetails.facingDirection, transform.position);
            }
            collidedObjects.Add(collision.gameObject);
        }

    }
}
