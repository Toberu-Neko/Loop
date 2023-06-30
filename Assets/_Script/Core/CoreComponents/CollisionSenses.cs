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
    public Transform HeadCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(headCheck, transform.parent.name);
        private set => headCheck = value;
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

    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform headCheck;
    #endregion

    [Tooltip("這個的值要略小於Collider的寬度, 不然碰到牆壁時會卡住.")]
    [SerializeField] private Vector2 groundCheckV2;

    [SerializeField] private Vector2 ceilingCheckV2;
    [SerializeField] private Vector2 headCheckV2;
    
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatform;

    public bool SolidCeiling 
    {
        get => Physics2D.BoxCast(CeilingCheck.position, ceilingCheckV2, 0f, Vector2.up, 0.1f, whatIsGround - whatIsPlatform);
    }
    public bool Ground
    {
        get => Physics2D.BoxCast(GroundCheck.position, groundCheckV2, 0f, Vector2.down, 0.1f, whatIsGround);
    }
    public RaycastHit2D HeadPlatform
    {
        get => Physics2D.BoxCast(HeadCheck.position, headCheckV2, 0f, Vector2.up, 0.1f, whatIsPlatform);
    }
    public RaycastHit2D GroundPlatform
    {
        get => Physics2D.BoxCast(GroundCheck.position, groundCheckV2, 0f, Vector2.down, 0.1f, whatIsPlatform);
    }
    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround - whatIsPlatform);
    }
    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround - whatIsPlatform);
    }
    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround - whatIsPlatform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if (WallCheck) Gizmos.DrawLine(WallCheck.position, WallCheck.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckHorizontal) Gizmos.DrawLine(LedgeCheckHorizontal.position, LedgeCheckHorizontal.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckVertical) Gizmos.DrawLine(LedgeCheckVertical.position, LedgeCheckVertical.position + Vector3.down * wallCheckDistance);

        if (GroundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GroundCheck.position, groundCheckV2);
        }
        if (GroundCheck && HeadCheck && CeilingCheck)
        {
            Gizmos.DrawWireCube(HeadCheck.position, headCheckV2);
            Gizmos.DrawWireCube(CeilingCheck.position, ceilingCheckV2);
        }
    }
}
