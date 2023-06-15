using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public FiniteStateMachine StateMachine { get; private set; }
    public D_Entity EntityData;
    public Animator Anim { get; private set; }
    public int LastDamageDirection { get; private set; }
    public AnimationToStatemachine AnimationToStatemachine { get; private set; }
    public Core Core { get; private set; }


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
        StateMachine.CurrentState.LogicUpdate();

        Anim.SetFloat("yVelocity", Core.Movement.RB.velocity.y);

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
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * EntityData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * EntityData.ledgeCheckDistance));
        }


        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.maxAgroDistance), 0.2f);

    }
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspcae.Set(Core.Movement.RB.velocity.x, velocity);
        Core.Movement.RB.velocity = velocityWorkspcae;
    }
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = EntityData.stunResistance;
    }

    public virtual void Damage(AttackDetails details)
    {
        lastDamageTime = Time.time;

        currentHealth -= details.damageAmount;
        currentStunResistance -= details.stunDamageAmount;

        DamageHop(EntityData.damageHopSpeed);

        Instantiate(EntityData.hitParticle, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (details.position.x > transform.position.x)
        {
            LastDamageDirection = -1;
        }
        else
        {
            LastDamageDirection = 1;
        }

        if(currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if(currentHealth <= 0)
        {
            isDead = true;
        }
    }
}
