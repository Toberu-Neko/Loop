using System;
using UnityEngine;

/// <summary>
/// All rigidbody movement related stuff.
/// </summary>
public class Movement : CoreComponent
{
    #region Variables
    public Rigidbody2D RB { get; private set; }
    private RigidbodyType2D orgRBBodyType;

    // For CollisionSenses, for decresing dependency.
    public Slope Slope { get; set; }
    public int FacingDirection { get; private set; }
    public bool CanSetVelocity { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    public Transform ParentTransform { get; private set; }
    public Vector2 TimeStopVelocity { get; private set; }
    public Vector2 TimeSlowVelocity { get; private set; }
    public float OrginalGravityScale { get; set; }
    private Vector2 velocityWorkspace;
    private float timeSlowOrgGravityScale;
    private float gravityWorkspace;
    private Vector2 previousPosition;

    public event Action OnFlip;
    public event Action OnStuck;

    private Stats stats;
    private bool inKinematicState;
    private Vector3 v3WorkSpace;
    #endregion

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();

        ParentTransform = core.transform.parent;
        RB = GetComponentInParent<Rigidbody2D>();
        stats = core.GetCoreComponent<Stats>();
    }

    private void OnEnable()
    {
        #region Initialization
        Slope = new();
        previousPosition = Vector2.zero;
        velocityWorkspace = Vector2.zero;
        TimeStopVelocity = Vector2.zero;
        TimeSlowVelocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
        FacingDirection = 1;
        inKinematicState = false;

        CanSetVelocity = true;

        if(OrginalGravityScale == 0)
        {
            OrginalGravityScale = RB.gravityScale;
        }

        orgRBBodyType = RB.bodyType;
        #endregion

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
            // Disable gravity when on slope.
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
    #endregion

    public void Teleport(Vector2 position)
    {
        RB.position = position;
    }

    #region Time Stop
    private void HandleTimeStopStart()
    {
        CurrentVelocity = RB.velocity;
        TimeStopVelocity = CurrentVelocity;
        SetRBKinematic();
        SetVelocityZero();
    }

    private void HandleTimeStopEnd()
    {
        SetRBDynamic();
        SetVelocity(TimeStopVelocity);
    }
    
    /// <summary>
    /// For changing the velocity of the object in time stop.
    /// </summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// For changing the velocity of the object in time slow.
    /// </summary>
    /// <param name="value"></param>
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
            RB.gravityScale = gravityWorkspace * stats.TimeSlowMultiplier;
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

    /// <summary>
    /// Sets the velocity of the rigidbody, with the given velocity, angle and direction.
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="angle">Normalized vector2</param>
    /// <param name="direction"> 1 = right, -1 = left</param>
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);

        SetFinalVelocity();
    }

    /// <summary>
    /// Sets the velocity of the rigidbody, with the given velocity and direction.
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="direction"> Normalized vector2</param>
    public void SetVelocity(float velocity, Vector2 direction)
    {
        velocityWorkspace = direction * velocity;

        SetFinalVelocity();
    }

    /// <summary>
    /// Sets the velocity of the rigidbody, with the given velocity.
    /// </summary>
    /// <param name="VectorVelocity">Unnormalized vector2</param>
    public void SetVelocity(Vector2 VectorVelocity)
    {
        velocityWorkspace = VectorVelocity;

        SetFinalVelocity();
    }

    /// <summary>
    /// Sets the X velocity of the rigidbody, with the given velocity and direction.
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="ignoreSlope"> Can be used when flying, to ignore the slope.</param>
    public void SetVelocityX(float velocity, bool ignoreSlope = false)
    {
        velocityWorkspace.Set(velocity, CurrentVelocity.y);

        if (Slope.IsOnSlope && !ignoreSlope && Slope.NormalPrep!= Vector2.zero)
        {
            SetVelocity(velocity, -Slope.NormalPrep);
            return;
        }

        SetFinalVelocity();
    }

    /// <summary>
    /// Sets the Y velocity of the rigidbody, with the given velocity.
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(CurrentVelocity.x, velocity);

        SetFinalVelocity();
    }

    /// <summary>
    /// Sets the velocity of the rigidbody to zero.
    /// </summary>
    public void SetVelocityZero()
    {
        velocityWorkspace = Vector2.zero;

        SetFinalVelocity();
    }

    /// <summary>
    /// Sets the angular velocity of the rigidbody, primarily used for rotating objects.
    /// </summary>
    /// <param name="velocity"></param>
    public void SetAngularVelocity(float velocity)
    {
        RB.angularVelocity = velocity;
    }

    /// <summary>
    /// Cauclates the final velocity, and check if is in time stop stats, and sets it to the rigidbody.
    /// </summary>
    private void SetFinalVelocity()
    {
        if (stats.IsTimeSlowed)
        {
            SetTimeSlowVelocity(velocityWorkspace);
        }
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
            Vector2 velocity = new(velocityWorkspace.x * stats.AnimationSpeed, velocityWorkspace.y * stats.AnimationSpeed);

            if(stats.IsTimeSlowed)
            {
                RB.velocity = velocity;
                CurrentVelocity = velocity;
            }
            else
            {
                RB.velocity = velocity;
                CurrentVelocity = velocity;
            }
        }
    }

    /// <summary>
    /// Mainly used for knockback, to make object uncontrollable.
    /// </summary>
    /// <param name="a"></param>
    public void SetCanSetVelocity(bool a)
    {
        CanSetVelocity = a;
    }
    #endregion

    #region Flip and Turn
    /// <summary>
    /// Checks if the facing direction should be flipped, and flips it if needed.
    /// </summary>
    /// <param name="xInput"></param>
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    /// <summary>
    /// For player and enemy, flips the facing direction.
    /// </summary>
    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180f, 0.0f);

        float roundedAngle = Mathf.Round(RB.transform.eulerAngles.y * 1000000f) / 1000000f;
        v3WorkSpace.Set(0f, roundedAngle, 0f);
        RB.transform.eulerAngles = v3WorkSpace;

        OnFlip?.Invoke();
    }

    /// <summary>
    /// For bullets and other objects, change through angle and set angle.
    /// </summary>
    /// <param name="angle"></param>
    public void Turn(float angle = 180f)
    {
        v3WorkSpace.Set(0f, 0f, angle);
        RB.transform.eulerAngles += v3WorkSpace; 
    }
    #endregion

    #region RB Kinematic Settings
    public void SetRBKinematic()
    {
        inKinematicState = true;
        RB.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetRBDynamic()
    {
        if(orgRBBodyType == RigidbodyType2D.Kinematic)
        {
            return;
        }
        inKinematicState = false;
        RB.bodyType = RigidbodyType2D.Dynamic;
    }

    public void SetKnockbackDynamic()
    {
        RB.bodyType = RigidbodyType2D.Dynamic;
    }

    public void SetKnockbackKinematic()
    {
        if(inKinematicState)
        {
            RB.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    #endregion
}
