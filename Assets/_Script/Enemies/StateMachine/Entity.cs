using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    public FiniteStateMachine StateMachine { get; private set; }
    public D_Entity EntityData;
    public int FacingDirection { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public GameObject AliveGO { get; private set; }
    public int LastDamageDirection { get; private set; }
    public AnimationToStatemachine AnimationToStatemachine { get; private set; }
    // public Core core { get; private set; }


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
        AliveGO = transform.Find("Alive").gameObject;
        RB = AliveGO.GetComponent<Rigidbody2D>();
        Anim = AliveGO.GetComponent<Animator>();
        AnimationToStatemachine = AliveGO.GetComponent<AnimationToStatemachine>();

        StateMachine = new();

        FacingDirection = 1;
        currentHealth = EntityData.maxHealth;
        currentStunResistance = EntityData.stunResistance;
    }
    public virtual void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

        if(Time.time >= lastDamageTime + EntityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }
    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspcae.Set(FacingDirection * velocity, RB.velocity.y);
        RB.velocity = velocityWorkspcae;
    }
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspcae.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = velocityWorkspcae;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, AliveGO.transform.right, EntityData.wallCheckDistance, EntityData.whatIsGround);
    }
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, EntityData.ledgeCheckDistance, EntityData.whatIsGround);
    }
    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, EntityData.minAgroDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, EntityData.maxAgroDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, EntityData.closeRangeActionDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, EntityData.groundCheckRadius, EntityData.whatIsGround);
    }
    public virtual void Flip()
    {
        FacingDirection *= -1;
        AliveGO.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * FacingDirection * EntityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * EntityData.ledgeCheckDistance));

        if(AliveGO != null)
        {
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(AliveGO.transform.right * EntityData.closeRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(AliveGO.transform.right * EntityData.minAgroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(AliveGO.transform.right * EntityData.maxAgroDistance), 0.2f);
        }

    }
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspcae.Set(RB.velocity.x, velocity);
        RB.velocity = velocityWorkspcae;
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

        Instantiate(EntityData.hitParticle, AliveGO.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (details.position.x > AliveGO.transform.position.x)
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
