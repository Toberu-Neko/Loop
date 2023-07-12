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
    public Transform WallBackCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallBackCheck, transform.parent.name);
        private set => wallBackCheck = value;
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
    [SerializeField] private Transform wallBackCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform headCheck;
    #endregion

    [Tooltip("這個的值要略小於Collider的寬度, 不然碰到牆壁時會卡住.")]
    [SerializeField] private Vector2 groundCheckV2;

    [SerializeField] private Vector2 ceilingCheckV2;
    [SerializeField] private Vector2 headCheckV2;

    [SerializeField] private float slopeCheckDistance = 0.5f;
    [SerializeField] private float slopeMaxAngle;
    
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float ledgeCheckDistance = 1f;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatform;

    private Slope slope = new();
    public bool SolidCeiling 
    {
        get => Physics2D.BoxCast(CeilingCheck.position, ceilingCheckV2, 0f, Vector2.up, 0.1f, whatIsGround - whatIsPlatform);
    }
    public bool Ground
    {
        get => Physics2D.BoxCast(GroundCheck.position, groundCheckV2, 0f, Vector2.down, 0.1f, whatIsGround);
    }
    public Slope Slope
    {
        get
        {
            RaycastHit2D hit = Physics2D.Raycast(GroundCheck.position, Vector2.down, slopeCheckDistance, whatIsGround);
            RaycastHit2D hitFront = Physics2D.Raycast(GroundCheck.position, Vector2.right * Movement.FacingDirection, slopeCheckDistance, whatIsGround);
            RaycastHit2D hitBack = Physics2D.Raycast(GroundCheck.position, Vector2.right * -Movement.FacingDirection, slopeCheckDistance, whatIsGround);

            if (hitFront)
            {
                slope.SetSideAngle(Vector2.Angle(hitFront.normal, Vector2.up));
            }
            else if (hitBack)
            {
                slope.SetSideAngle(Vector2.Angle(hitBack.normal, Vector2.up));
            }
            else
            {
                slope.SetSideAngle(0f);
                slope.SetIsOnSlope(false);
            }

            if (!hit)
            {
                return slope;
            }
            else
            {
                Vector2 normalPerp = Vector2.Perpendicular(hit.normal).normalized;
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                slope.Set(normalPerp, slopeAngle);
                return slope;
            }
        }
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
    public bool WallFrontLong
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance * 3f, whatIsGround - whatIsPlatform);
    }
    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround - whatIsPlatform);
    }
    public bool WallBackLong
    {
        get => Physics2D.Raycast(WallBackCheck.position, -Vector2.right * Movement.FacingDirection, wallCheckDistance * 3f, whatIsGround - whatIsPlatform);
    }
    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, ledgeCheckDistance, whatIsGround - whatIsPlatform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if (WallCheck) Gizmos.DrawLine(WallCheck.position, WallCheck.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckHorizontal) Gizmos.DrawLine(LedgeCheckHorizontal.position, LedgeCheckHorizontal.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckVertical) Gizmos.DrawLine(LedgeCheckVertical.position, LedgeCheckVertical.position + Vector3.down * ledgeCheckDistance);

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

public class Slope
{
    public Vector2 NormalPrep { get; private set; }
    public float DownAngle { get; private set; }
    private float downAngleOld;
    public bool IsOnSlope { get; private set; }
    public float SideAngle { get; private set; }

    public Slope(Vector2 slopeNormal, float downAngle)
    {
        NormalPrep = slopeNormal;
        DownAngle = downAngle;
    }

    public Slope()
    {
        NormalPrep = Vector2.zero;
        DownAngle = 0f;
    }

    public void SetSideAngle(float angle)
    {
        SideAngle = angle;

        if (angle > 10f && angle < 80f)
            IsOnSlope = true;
        else
            IsOnSlope = false;
    }

    public void SetIsOnSlope(bool isOnSlope)
    {
        IsOnSlope = isOnSlope;
    }

    public void Set(Vector2 slopeNormal, float slopeAngle)
    {
        NormalPrep = slopeNormal;
        DownAngle = slopeAngle;

        if (DownAngle != downAngleOld)
        {
            IsOnSlope = true;
        }
        if(DownAngle < 10f)
        {
            IsOnSlope = false;
        }

        downAngleOld = DownAngle;

    }
}