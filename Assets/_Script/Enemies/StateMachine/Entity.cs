using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine StateMachine { get; private set; }
    public D_Entity EntityData;
    public Animator Anim { get; private set; }
    public int LastDamageDirection { get; private set; }
    public AnimationToStatemachine AnimationToStatemachine { get; private set; }
    public Core Core { get; private set; }
    private Movement movement;


    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;

    protected Stats stats;

    private float lastDamageTime;

    private Vector2 velocityWorkspcae;

    protected bool isStunned;
    protected bool isDead;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        movement = Core.GetCoreComponent<Movement>();
        stats = Core.GetCoreComponent<Stats>();

        Anim = GetComponent<Animator>();
        AnimationToStatemachine = GetComponent<AnimationToStatemachine>();

        StateMachine = new();
    }
    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        Anim.SetFloat("yVelocity", movement.RB.velocity.y);

        if(Time.time >= lastDamageTime + EntityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }
    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.minAgroDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.maxAgroDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.closeRangeActionDistance, EntityData.whatIsPlayer);
    }

    public virtual void OnDrawGizmos()
    {
        if (Core != null)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * movement.FacingDirection * EntityData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * EntityData.ledgeCheckDistance));
        }


        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.maxAgroDistance), 0.2f);

    }
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspcae.Set(movement.RB.velocity.x, velocity);
        movement.RB.velocity = velocityWorkspcae;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }
}
