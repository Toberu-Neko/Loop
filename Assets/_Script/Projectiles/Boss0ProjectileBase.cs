using System;
using UnityEngine;

// This is the base class for all projectiles that will do something when they collide with something
public class Boss0ProjectileBase : MonoBehaviour, IFireable
{
    [SerializeField] protected LayerMask interactables;
    [SerializeField] private Rigidbody2D RB;

    protected event Action<Collision2D> OnAction;

    private void OnEnable()
    {
        RB.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Fire(Vector2 fireDirection, ProjectileDetails details)
    {
        RB.velocity = fireDirection * details.speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == (interactables | (1 << collision.gameObject.layer)))
        {
            Debug.Log("Enter!");
            OnAction?.Invoke(collision);
        }
    }
}
