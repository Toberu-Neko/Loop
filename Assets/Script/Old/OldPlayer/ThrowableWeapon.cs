using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
	public Vector2 direction;
    [SerializeField] private float speed = 10f;

	private bool hasHit = false;
	private Rigidbody2D rig;

    private void Awake()
    {
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
        rig = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
		if (!hasHit)
            rig.velocity = direction * speed;
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
