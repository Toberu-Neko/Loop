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
    private Movement Movement => movement ? movement : Core.GetCoreComponent<Movement>();
    private Movement movement;


    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    private Vector2 velocityWorkspcae;

    protected bool isStunned;
    protected bool isDead;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        Anim = GetComponent<Animator>();
        AnimationToStatemachine = GetComponent<AnimationToStatemachine>();

        StateMachine = new();

        currentHealth = EntityData.maxHealth;
        currentStunResistance = EntityData.stunResistance;
    }
    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        Anim.SetFloat("yVelocity", Movement.RB.velocity.y);

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
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * EntityData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * EntityData.ledgeCheckDistance));
        }


        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.maxAgroDistance), 0.2f);

    }
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspcae.Set(Movement.RB.velocity.x, velocity);
        Movement.RB.velocity = velocityWorkspcae;
    }
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = EntityData.stunResistance;
    }
}
