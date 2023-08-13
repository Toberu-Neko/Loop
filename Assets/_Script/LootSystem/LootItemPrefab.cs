using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemPrefab : MonoBehaviour
{
    [HideInInspector] public LootDetails lootDetails;

    [SerializeField] private Transform groundDetector;
    [SerializeField] private Vector2 groundDetectorSize;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;

    private bool IsGrounded
    {
        get
        {
            return Physics2D.BoxCast((Vector2)groundDetector.position, groundDetectorSize, 0f, Vector2.down, 0.1f, whatIsGround);
        }
    }

    private void OnEnable()
    {
        gameObject.layer = 15;
        col.isTrigger = false;
        rb.isKinematic = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)groundDetector.position, groundDetectorSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGrounded)
        {
            col.isTrigger = true;
            rb.isKinematic = true;
            gameObject.layer = 14;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collect " + lootDetails.name);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}


