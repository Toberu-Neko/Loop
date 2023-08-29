using System;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; private set; }

    public bool CanSetVelocity { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }
    public Transform ParentTransform { get; private set; }

    private Vector2 velocityWorkspace;
    public Vector2 TimeStopVelocity { get; private set; }
    private float orginalGrag;

    public Vector2 TimeSlowVelocity { get; private set; }
    private float timeSlowOrgGravityScale;
    public float OrginalGravityScale { get; set; }
    private float gravityWorkspace;

    private Vector2 previousPosition;

    public event Action OnFlip;
    public event Action OnStuck;

    public Slope Slope { get; set; }
    private Stats stats;

    protected override void Awake()
    {
        base.Awake();

        ParentTransform = core.transform.parent;
        RB = GetComponentInParent<Rigidbody2D>();
        stats = core.GetCoreComponent<Stats>();
    }


    private void OnEnable()
    {
        Slope = new();
        previousPosition = Vector2.zero;
        velocityWorkspace = Vector2.zero;
        TimeStopVelocity = Vector2.zero;
        TimeSlowVelocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;

        orginalGrag = RB.drag;

        FacingDirection = 1;

        CanSetVelocity = true;

        stats.OnTimeStopEnd += HandleTimeStopEnd;
        stats.OnTimeStopStart += HandleTimeStopStart;

        stats.OnTimeSlowStart += HandleTimeSlowStart;
        stats.OnTimeSlowEnd += HandleTimeSlowEnd;
    }


    private void OnDisable()
    {
        SetGravityOrginal();


        stats.OnTimeStopEnd -= HandleTimeStopEnd;
        stats.OnTimeStopStart -= HandleTimeStopStart;

        stats.OnTimeSlowStart -= HandleTimeSlowStart;
        stats.OnTimeSlowEnd -= HandleTimeSlowEnd;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CurrentVelocity = RB.velocity;

        if (Slope.hasCollisionSenses)
        {
            if (Slope.IsOnSlope)
            {
                SetGravityZero();
            }
            else
            {
                SetGravityOrginal();
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (velocityWorkspace != Vector2.zero)
        {
            if (previousPosition == (Vector2)ParentTransform.position)
            {
                OnStuck?.Invoke();
            }
            previousPosition = ParentTransform.position;
        }
    }

    #region Time Stop
    private void HandleTimeStopStart()
    {
        CurrentVelocity = RB.velocity;
        TimeStopVelocity = CurrentVelocity;
        RB.isKinematic = true;
        SetVelocityZero();
    }

    private void HandleTimeStopEnd()
    {
        RB.isKinematic = false;
        SetVelocity(TimeStopVelocity);
    }
    
    public void SetTimeStopVelocity(Vector2 value)
    {
        TimeStopVelocity = value;
    }
    #endregion

    #region TimeSlow
    private void HandleTimeSlowStart()
    {
        gravityWorkspace = RB.gravityScale;
        TimeSlowVelocity = CurrentVelocity;
        SetFinalGravity();
    }

    private void HandleTimeSlowEnd()
    {
        gravityWorkspace = timeSlowOrgGravityScale;
        SetVelocity(TimeSlowVelocity);
    }

    public void SetTimeSlowVelocity(Vector2 value)
    {
        TimeSlowVelocity = value;
    }

    #endregion

    #region Set Gravity
    public void SetGravityZero()
    {
        gravityWorkspace = 0.0f;
        SetFinalGravity();
    }

    public void SetGravityOrginal()
    {
        gravityWorkspace = OrginalGravityScale;
        SetFinalGravity();
    }

    private void SetFinalGravity()
    {
        if (stats.IsTimeSlowed)
        {
            timeSlowOrgGravityScale = gravityWorkspace;
            RB.gravityScale = gravityWorkspace * stats.TimeSlowMultiplier * stats.TimeSlowMultiplier;
        }
        else
        {
            RB.gravityScale = gravityWorkspace;
        }
    }
    #endregion

    #region Set Velocity

    public void SetPosition(Vector2 position, Quaternion rotation, int facingDirection)
    {
        ParentTransform.SetPositionAndRotation(position, rotation);
        FacingDirection = facingDirection;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);

        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        velocityWorkspace = direction * velocity;

        SetFinalVelocity();
    }

    public void SetVelocity(Vector2 VectorVelocity)
    {
        velocityWorkspace = VectorVelocity;

        SetFinalVelocity();
    }
    public void SetVelocityX(float velocity, bool ignoreSlope = false)
    {
        velocityWorkspace.Set(velocity, CurrentVelocity.y);

        if (Slope.IsOnSlope && !ignoreSlope)
        {
            SetVelocity(velocity, -Slope.NormalPrep);
            return;
        }

        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(CurrentVelocity.x, velocity);

        SetFinalVelocity();
    }
    public void SetVelocityZero()
    {
        velocityWorkspace = Vector2.zero;

        SetFinalVelocity();
    }

    public void SetAngularVelocity(float velocity)
    {
        RB.angularVelocity = velocity;
    }

    private void SetFinalVelocity()
    {
        if(stats.IsTimeStopped)
        {
            RB.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
            return;
        }

        if (velocityWorkspace == Vector2.zero && RB.sharedMaterial != core.CoreData.fullFrictionMaterial && Slope.IsOnSlope)
        {
            RB.sharedMaterial = core.CoreData.fullFrictionMaterial;
        }
        else if (velocityWorkspace != Vector2.zero && RB.sharedMaterial != core.CoreData.noFrictionMaterial)
        {
            RB.sharedMaterial = core.CoreData.noFrictionMaterial;
        }


        if (CanSetVelocity)
        {
            if(stats.IsTimeSlowed)
            {
                RB.velocity = velocityWorkspace * stats.TimeSlowMultiplier;
                CurrentVelocity = velocityWorkspace * stats.TimeSlowMultiplier;
            }
            else
            {
                RB.velocity = velocityWorkspace;
                CurrentVelocity = velocityWorkspace;
            }
        }
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    public void SetDragZero()
    {
        RB.drag = 0.0f;
    }

    public void SetDragOrginal()
    {
        RB.drag = orginalGrag;
    }

    public void SetCanSetVelocity(bool a)
    {
        CanSetVelocity = a;
    }


    private Vector3 v3WorkSpace;

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180, 0.0f);

        float roundedAngle = Mathf.Round(RB.transform.eulerAngles.y * 1000000f) / 1000000f;
        v3WorkSpace.Set(0f, roundedAngle, 0f);
        RB.transform.eulerAngles = v3WorkSpace;

        OnFlip?.Invoke();
        //TODO: Fixed Rotate, but remove if lag.
    }

    public void Turn()
    {
        v3WorkSpace.Set(0f, 0f, 180f);
        RB.transform.eulerAngles += v3WorkSpace; 
    }

    public void Teleport(Vector2 position)
    {
        RB.position = position;
    }
    #endregion
}
