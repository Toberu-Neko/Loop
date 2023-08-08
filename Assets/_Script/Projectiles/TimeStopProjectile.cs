using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopProjectile : MonoBehaviour
{
    [SerializeField] private Core core;
    private Movement movement;
    private float stopTime;

    [SerializeField] private float explodeRadius = 2f;
    [SerializeField] private LayerMask whatIsInteractable;
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Collider2D col;
    [SerializeField] private GameObject explodeRange;
    private Transform fireTransform;
    private bool returnToPlayer;
    [SerializeField] private float returnVelocity = 5f;
    [SerializeField] private float returnAngularVelocity = 1000f;

    public event Action OnReturnToPlayer;

    private void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
        explodeRange.SetActive(false);
        returnToPlayer = false;
    }

    public void Fire(float velocity, Vector2 direction, float stopTime, float gravityScale, Transform fireTransform)
    {
        movement.SetVelocity(velocity, direction);
        this.stopTime = stopTime;
        RB.gravityScale = gravityScale;
        this.fireTransform = fireTransform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(collision.gameObject.name + "Layer = " + collision.gameObject.layer);
        if(collision.gameObject.layer == 6 || collision.gameObject.layer == 13)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explodeRadius, Vector2.zero, 0, whatIsInteractable);

            foreach(RaycastHit2D h in hit)
            {
                if(h.collider.gameObject.TryGetComponent(out ITimeStopable stopable))
                {
                    stopable.DoTimeStop(stopTime);
                }
            }
            explodeRange.SetActive(true);
            RB.simulated = false;
            col.isTrigger = true;
            gameObject.layer = 8;

            Invoke(nameof(ReturnToPlayer), 1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnReturnToPlayer?.Invoke();
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (returnToPlayer)
        {
            Vector2 direction = (Vector2)fireTransform.position - RB.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            movement.SetAngularVelocity(-rotateAmount * returnAngularVelocity);
            movement.SetVelocity(transform.up * returnVelocity);

            returnVelocity += Time.fixedDeltaTime * 20f;
        }
    }

    private void ReturnToPlayer()
    {
        returnToPlayer = true;
        RB.simulated = true;
        explodeRange.SetActive(false);
        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
