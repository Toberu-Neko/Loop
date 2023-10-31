using UnityEngine;

public class CollisionSenses : CoreComponent
{
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
    public Transform ChangeColliderWallCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(changeColliderWallCheck, transform.parent.name);
        private set => changeColliderWallCheck = value;
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
    [SerializeField] private Transform changeColliderWallCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform wallBackCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform headCheck;
    [SerializeField] private Transform slopeCheckVerticalFront;
    [SerializeField] private Transform slopeCheckVerticalBack;
    #endregion

    [Tooltip("這個的值要略小於Collider的寬度, 不然碰到牆壁時會卡住.")]
    [SerializeField] private Vector2 groundCheckV2;
    [SerializeField] private Vector2 slopeCheckV2;
    [SerializeField] private Vector2 changeColliderWallCheckV2;
    [SerializeField] private Vector2 changeColliderWallCheckV2Crouch;

    [SerializeField] private Vector2 ceilingCheckV2;
    [SerializeField] private Vector2 headCheckV2;

    [SerializeField] private float slopeCheckDistance = 1.2f;
    [SerializeField] private float slopeCheckDistanceBack = 0.25f;
    [SerializeField] private float slopeMaxAngle;

    
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float ledgeCheckDistance = 1f;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsClimbableWall;
    [SerializeField] protected LayerMask whatIsUnclimbableWall;
    [SerializeField] private LayerMask whatIsPlatform;

    private Slope slope = new();

    protected override void Awake()
    {
        base.Awake();

        movement = core.GetCoreComponent<Movement>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movement.Slope = Slope;
    }


    public bool SolidCeiling 
    {
        get => Physics2D.BoxCast(CeilingCheck.position, ceilingCheckV2, 0f, Vector2.up, 0.1f, whatIsGround - whatIsPlatform);
    }
    public bool Ground
    {
        get
        {
            if(!Slope.IsOnSlope)
            {
                return Physics2D.BoxCast(GroundCheck.position, groundCheckV2, 0f, Vector2.down, 0.1f, whatIsGround);
            }
            else
            {
                return Physics2D.BoxCast(GroundCheck.position, slopeCheckV2, 0f, Vector2.down, 0.1f, whatIsGround);
            }
        }
    }

    public Slope Slope
    {
        get
        {
            slope = new();
            RaycastHit2D hitVerticleFront = Physics2D.Raycast(slopeCheckVerticalFront.position, Vector2.down, slopeCheckDistance, whatIsGround);
            RaycastHit2D hitVerticalBack = Physics2D.Raycast(slopeCheckVerticalBack.position, Vector2.down, slopeCheckDistance, whatIsGround);
            RaycastHit2D hitFront = Physics2D.Raycast(GroundCheck.position, Vector2.right * movement.FacingDirection, slopeCheckDistance, whatIsGround);
            RaycastHit2D hitBack = Physics2D.Raycast(GroundCheck.position, Vector2.right * -movement.FacingDirection, slopeCheckDistance, whatIsGround);
            slope.hasCollisionSenses = true;

            float frontAngleHor = Vector2.Angle(hitFront.normal, Vector2.up);
            float backAngleHor = Vector2.Angle(hitBack.normal, Vector2.up);

            /*
            if (hitFront)
            {
                if(Vector2.Angle(hitFront.normal, Vector2.up) <= slopeMaxAngle)
                {
                    slope.SetSideAngle(Vector2.Angle(hitFront.normal, Vector2.up));
                }
                else
                {
                    slope.SetSideAngle(0f);
                    slope.SetIsOnSlope(false);
                }
            }
            else if (hitBack)
            {
                if(Vector2.Angle(hitBack.normal, Vector2.up) <= slopeMaxAngle)
                {
                    slope.SetSideAngle(Vector2.Angle(hitBack.normal, Vector2.up));
                }
                else
                {
                    slope.SetSideAngle(0f);
                    slope.SetIsOnSlope(false);
                }
            }
            else
            {
                slope.SetSideAngle(0f);
                slope.SetIsOnSlope(false);
            }
            */
            float frontAngle = Vector2.Angle(hitVerticleFront.normal, Vector2.up);
            float backAngle = Vector2.Angle(hitVerticalBack.normal, Vector2.up);


            if (((frontAngle == 0f && backAngle == 0f) || (frontAngleHor == 0f && backAngleHor == 0f)))
            {
                slope.SetIsOnSlope(false);
                return slope;
            }
            else
            {
                Vector2 normalPerp = Vector2.Perpendicular(hitVerticleFront.normal).normalized;

                slope.Set(normalPerp, frontAngle);

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

    public bool UnclimbableWallFront
    {
        get=> Physics2D.Raycast(GroundCheck.position, Vector2.right * movement.FacingDirection, slopeCheckDistance, whatIsUnclimbableWall);
    }
    public bool CanChangeCollider
    {
        get => !Physics2D.BoxCast(ChangeColliderWallCheck.position, changeColliderWallCheckV2, 0f, Vector2.right * movement.FacingDirection, 0.1f, whatIsGround);
    }

    public bool CrouchCanChangeCollider
    {
        get => !Physics2D.BoxCast(ChangeColliderWallCheck.position, changeColliderWallCheckV2Crouch, 0f, Vector2.right * movement.FacingDirection, 0.1f, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * movement.FacingDirection, wallCheckDistance, whatIsClimbableWall);
    }
    public bool WallFrontLong
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * movement.FacingDirection, wallCheckDistance * 3f, whatIsClimbableWall);
    }
    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -movement.FacingDirection, wallCheckDistance, whatIsClimbableWall);
    }
    public bool WallBackLong
    {
        get => Physics2D.Raycast(WallBackCheck.position, -Vector2.right * movement.FacingDirection, wallCheckDistance * 3f, whatIsClimbableWall);
    }
    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * movement.FacingDirection, wallCheckDistance, whatIsGround - whatIsPlatform);
    }
    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, ledgeCheckDistance, whatIsGround);
    }

    public bool IsDetectingWall(Vector2 direction, float distance)
    {
        return Physics2D.Raycast(movement.ParentTransform.position, direction, distance, whatIsClimbableWall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (WallCheck)
        {
            Gizmos.DrawLine(WallCheck.position, WallCheck.position + Vector3.right * wallCheckDistance);
        }
        if (ChangeColliderWallCheck)
        {
            Gizmos.DrawWireCube(ChangeColliderWallCheck.position, changeColliderWallCheckV2);
            Gizmos.DrawWireCube(ChangeColliderWallCheck.position, changeColliderWallCheckV2Crouch);
        }
        if (LedgeCheckHorizontal) Gizmos.DrawLine(LedgeCheckHorizontal.position, LedgeCheckHorizontal.position + Vector3.right * wallCheckDistance);
        if (LedgeCheckVertical) Gizmos.DrawLine(LedgeCheckVertical.position, LedgeCheckVertical.position + Vector3.down * ledgeCheckDistance);

        if (GroundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GroundCheck.position, groundCheckV2);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(GroundCheck.position, slopeCheckV2);
            Gizmos.DrawLine(GroundCheck.position, GroundCheck.position + Vector3.right * slopeCheckDistance);
        }
        if (slopeCheckVerticalFront)
        {
            Gizmos.DrawLine(slopeCheckVerticalFront.position, slopeCheckVerticalFront.position + Vector3.down * slopeCheckDistance);
        }
        if(slopeCheckVerticalBack)
        {
            Gizmos.DrawLine(slopeCheckVerticalBack.position, slopeCheckVerticalBack.position + Vector3.down * slopeCheckDistanceBack);
        }

        if (GroundCheck && HeadCheck && CeilingCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(HeadCheck.position, headCheckV2);
            Gizmos.DrawWireCube(CeilingCheck.position, ceilingCheckV2);
        }
    }
}

public class Slope
{
    public Vector2 NormalPrep { get; private set; }
    public float DownAngle { get; private set; }
    public bool IsOnSlope { get; private set; }
    public bool hasCollisionSenses;


    public Slope()
    {
        NormalPrep = Vector2.zero;
        DownAngle = 0f;
        IsOnSlope = false;
        hasCollisionSenses = false;
    }


    public void SetIsOnSlope(bool isOnSlope)
    {
        IsOnSlope = isOnSlope;
    }

    public void Set(Vector2 slopeNormal, float slopeAngle)
    {
        NormalPrep = slopeNormal;
        DownAngle = slopeAngle;

        IsOnSlope = true;
    }
}