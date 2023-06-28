using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; private set; }

    public bool CanSetVelocity { get; set; }

    private float orginalGrag;
    private float orginalGravityScale;

    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 velocityWorkspace;
    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();
        orginalGrag = RB.drag;
        orginalGravityScale = RB.gravityScale;

        FacingDirection = 1;
        CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    #region Set Functions
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
    public void SetVelocityX(float velocity)
    {
        velocityWorkspace.Set(velocity, CurrentVelocity.y);
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
        if (CanSetVelocity)
        {
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

        //TODO: Fixed Rotate but remove if lag.
    }
    #endregion
}
