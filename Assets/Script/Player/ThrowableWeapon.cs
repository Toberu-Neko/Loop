using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector2 direction;
    [SerializeField] private bool hasHit = false;
    [SerializeField] private float speed = 10f;

    void FixedUpdate()
    {
		if (!hasHit)
		GetComponent<Rigidbody2D>().velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
            Destroy(gameObject);
		}
		else if (!collision.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
