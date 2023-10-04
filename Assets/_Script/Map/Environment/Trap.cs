using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private Vector2 knockbackAngle;
    [SerializeField] private Transform teleportPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out IDamageable dam);
            dam?.Damage(damageAmount, collision.transform.position, false);
            collision.transform.position = teleportPos.position;
            UI_Manager.Instance.BlockPlayerSight();
        }
        else
        {
            collision.gameObject.TryGetComponent(out IDamageable dam);
            dam?.Damage(damageAmount, collision.transform.position, false);

            collision.gameObject.TryGetComponent(out IKnockbackable knock);
            knock?.Knockback(knockbackAngle, knockbackForce, new Vector2(collision.transform.position.x + Random.Range(-1f, 1f), collision.transform.position.y), false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out IDamageable dam);
            dam?.Damage(damageAmount, collision.transform.position, false);
            collision.transform.position = teleportPos.position;
            UI_Manager.Instance.BlockPlayerSight();
        }
        else
        {
            collision.gameObject.TryGetComponent(out IDamageable dam);
            dam?.Damage(damageAmount, collision.transform.position, false);

            collision.gameObject.TryGetComponent(out IKnockbackable knock);
            knock?.Knockback(knockbackAngle, knockbackForce, new Vector2(collision.transform.position.x + Random.Range(-1f, 1f), collision.transform.position.y), false);
        }
    }
}
