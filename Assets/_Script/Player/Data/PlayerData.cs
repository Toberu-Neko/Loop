using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("MoveState")]
    public float movementVelocity = 10f;

    [Header("JumpState")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("WallJumpState")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new(1, 2);

    [Header("InAirState")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    
    [Header("WallSlideState")]
    public float wallSlideVelocity = 3f;

    [Header("WallClimbState")]
    public float wallClimbVelocity = 3f;

    [Header("LedgeClimbState")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("DashState")]
    public float dashCooldown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("CrouchState")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 0.95f;
    public float standColliderHeight = 1.65f;
}
