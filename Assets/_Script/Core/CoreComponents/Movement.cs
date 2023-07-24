using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; private set; }

    public bool CanSetVelocity { get; set; }


    public Vector2 CurrentVelocity { get; private set; }
    private Transform parentTransform;
    public Slope Slope { get; set; } = new();

    private Vector2 velocityWorkspace;
    private Vector2 timeStopVelocity;
    private float orginalGrag;
    private float orginalGravityScale;

    public event Action OnFlip;

    private Stats stats;

    protected override void Awake()
    {
        base.Awake();

        parentTransform = core.transform.parent;
        RB = GetComponentInParent<Rigidbody2D>();
        stats = core.GetCoreComponent<Stats>();

        orginalGrag = RB.drag;
        orginalGravityScale = RB.gravityScale;

        FacingDirection = 1;
        CanSetVelocity = true;
        stats.OnTimeStart += HandleTimeStart;
        stats.OnTimeStop += HandleTimeStop;
    }

    private void HandleTimeStop()
    {
        CurrentVelocity = RB.velocity;
        timeStopVelocity = CurrentVelocity;
        RB.isKinematic = true;
        SetVelocityZero();
    }

    private void HandleTimeStart()
    {
        RB.isKinematic = false;
        SetVelocity(timeStopVelocity);
    }

    private void OnDisable()
    {
        stats.OnTimeStart -= HandleTimeStart;
        stats.OnTimeStop -= HandleTimeStop;
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    #region Set Functions

    public void SetPosition(Vector2 position, Quaternion rotation, int facingDirection)
    {
        parentTransform.SetPositionAndRotation(position, rotation);
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

    private void SetVelocity(Vector2 VectorVelocity)
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

    private void SetFinalVelocity()
    {
        if(stats.IsTimeStopped)
        {
            RB.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
            return;
        }
        if (CanSetVelocity)
        {
            if(velocityWorkspace == Vector2.zero && RB.sharedMaterial != core.CoreData.fullFrictionMaterial && Slope.IsOnSlope)
            {
                RB.sharedMaterial = core.CoreData.fullFrictionMaterial;
            }
            else if(velocityWorkspace != Vector2.zero && RB.sharedMaterial != core.CoreData.noFrictionMaterial)
            {
                RB.sharedMaterial = core.CoreData.noFrictionMaterial;
            }
            RB.velocity = velocityWorkspace;
            CurrentVelocity = velocityWorkspace;
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

    public void SetGravityZero()
    {
        RB.gravityScale = 0.0f;
    }

    public void SetGravityOrginal()
    {
        RB.gravityScale = orginalGravityScale;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180, 0.0f);

        float roundedAngle = Mathf.Round(RB.transform.eulerAngles.y * 1000000f) / 1000000f;
        RB.transform.eulerAngles = new Vector3(0f, roundedAngle, 0f);

        OnFlip?.Invoke();
        //TODO: Fixed Rotate, but remove if lag.
    }
    #endregion
}
