using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    private Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;

    #region Check Transforms

    public Transform GroundCheck 
    { 
        get => GenericNotImplementedError<Transform>.TryGet(groundCheck, transform.parent.name);
        private set => groundCheck = value; 
    }
    public Transform WallCheck {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, transform.parent.name);
        private set => wallCheck = value; 
    }
    public Transform LedgeCheckHorizontal
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, transform.parent.name);
        private set => ledgeCheckHorizontal = value; 
    }
    public Transform LedgeCheckVertical 
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, transform.parent.name);
        private set => ledgeCheckVertical = value; 
    }
    public Transform CeilingCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, transform.parent.name);
        private set => ceilingCheck = value; 
    }

    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck;
    #endregion

    [SerializeField] private float ceilingCheckRadius;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private LayerMask whatIsGround;

    public bool Ceiling 
    {
        get => Physics2D.OverlapCircle(CeilingCheck.position, ceilingCheckRadius, whatIsGround);
    }
    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
    }
    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if(GroundCheck) Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);
        if (CeilingCheck) Gizmos.DrawWireSphere(CeilingCheck.position, ceilingCheckRadius);
        if (WallCheck) Gizmos.DrawLine(WallCheck.position, WallCheck.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckHorizontal) Gizmos.DrawLine(LedgeCheckHorizontal.position, LedgeCheckHorizontal.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckVertical) Gizmos.DrawLine(LedgeCheckVertical.position, LedgeCheckVertical.position + Vector3.down * wallCheckDistance);
    }
}
