using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine StateMachine { get; private set; }
    public D_Entity EntityData;
    public int FacingDirection { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public GameObject AliveGO { get; private set; }
    public int LastDamageDirection { get; private set; }
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

        StateMachine = new();

        FacingDirection = 1;
    }
    public virtual void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        // Anim.SetFloat("yVelocity", RB.velocity.y);
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

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, AliveGO.transform.right, EntityData.wallCheckDistance, EntityData.whatIsGround);
    }
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, EntityData.ledgeCheckDistance, EntityData.whatIsGround);
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
    }
}
