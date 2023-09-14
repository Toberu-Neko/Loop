using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemPrefab : MonoBehaviour
{
    [HideInInspector] public LootSO lootSO;

    [SerializeField] private Transform groundDetector;
    [SerializeField] private Vector2 groundDetectorSize;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;

    private PlayerInventoryManager playerInventoryManager;

    private float startTime;
    private bool interactable;

    private bool IsGrounded
    {
        get
        {
            return Physics2D.BoxCast((Vector2)groundDetector.position, groundDetectorSize, 0f, Vector2.down, 0.1f, whatIsGround) && Time.time > startTime + 0.2f;
        }
    }

    private void OnEnable()
    {
        interactable = false;
        startTime = Time.time;
        gameObject.layer = 15;
        col.isTrigger = false;
        rb.isKinematic = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Invoke(nameof(Interectable), 2.5f);
    }

    private void Update()
    {
        if (IsGrounded && !interactable)
        {
            Interectable();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)groundDetector.position, groundDetectorSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventoryManager.Instance.AddChip(lootSO.itemDetails);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

    private void Interectable()
    {
        CancelInvoke(nameof(Interectable));

        interactable = true;
        col.isTrigger = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        gameObject.layer = 14;
    }
}


