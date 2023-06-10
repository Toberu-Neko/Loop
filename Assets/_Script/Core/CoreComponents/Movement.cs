using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 v2Workspace;
    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();

        FacingDirection = 1;
    }

    public void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    #region Set Functions
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        v2Workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = v2Workspace;
        CurrentVelocity = v2Workspace;
    }
    public void SetVelocity(float velocity, Vector2 direction)
    {
        v2Workspace = direction * velocity;
        RB.velocity = v2Workspace;
        CurrentVelocity = v2Workspace;
    }
    public void SetVelocityX(float velocity)
    {
        v2Workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = v2Workspace;
        CurrentVelocity = v2Workspace;
    }

    public void SetVelocityY(float velocity)
    {
        v2Workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = v2Workspace;
        CurrentVelocity = v2Workspace;
    }
    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    private void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
