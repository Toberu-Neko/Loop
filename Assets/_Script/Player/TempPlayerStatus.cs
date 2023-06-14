using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void Damage(AttackDetails details)
    {
        Debug.Log("Player damaged");

        if(details.position.x > transform.position.x)
        {
            Debug.Log("Player damaged from right");
            rb.velocity = new(-2, 1);
        }
        else
        {
            Debug.Log("Player damaged from left");
            rb.velocity = new(2, 1);

        }
    }
}
