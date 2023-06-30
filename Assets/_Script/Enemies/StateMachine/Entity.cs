using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Entity : MonoBehaviour
{
    [SerializeField] private Transform playerCheck;

    public FiniteStateMachine StateMachine { get; private set; }
    public D_Entity EntityData;

    public Core Core { get; private set; }
    private Movement movement;
    protected Stats stats;

    public AnimationToStatemachine AnimationToStatemachine { get; private set; }
    public Animator Anim { get; private set; }
    private Vector2 velocityWorkspcae;

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
    }
    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.DoChecks();
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
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.maxAgroDistance), 0.2f);
    }
    private void AnimationActionTrigger()
    {
        StateMachine.CurrentState.AnimationActionTrigger();
    }
    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }
}
