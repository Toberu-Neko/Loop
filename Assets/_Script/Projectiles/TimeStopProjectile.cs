using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopProjectile : MonoBehaviour
{
    [SerializeField] private Core core;
    private Movement movement;
    private float stopTime;

    [SerializeField] private float explodeRadius = 2f;
    [SerializeField] private LayerMask whatIsInteractable;
    private void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
    }

    public void Fire(float velocity, Vector2 direction, float stopTime)
    {
        movement.SetVelocity(velocity, direction);
        this.stopTime = stopTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(collision.gameObject.name + "Layer = " + collision.gameObject.layer);
        if(collision.gameObject.layer == 6 || collision.gameObject.layer == 13)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explodeRadius, Vector2.zero, 0, whatIsInteractable);

            foreach(RaycastHit2D h in hit)
            {
                // Debug.Log(h.collider.gameObject.name);
                if(h.collider.gameObject.TryGetComponent(out ITimeStopable stopable))
                {
                    Debug.Log("Get " + stopable.ToString());
                    stopable.DoTimeStop(stopTime);
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
