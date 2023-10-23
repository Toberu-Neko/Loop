using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public LayerMask whatIsEnemy;
    public LayerMask whatIsEnemyProjectile;
    public float gravityScale = 5f;

    [Header("Regen")]
    public float regenCost = 10f;
    public float regenAmount = 10f;

    [Header("MoveState")]
    [Tooltip("移動速度")]
    public float movementVelocity = 10f;

    [Header("JumpState")]
    [Tooltip("跳躍高度")]
    public float jumpVelocity = 15f;
    [Tooltip("跳躍數量")]
    public int amountOfJumps = 1;
    public float maxJumpTime = 0.25f;

    [Header("BlockState")]
    [Tooltip("防禦移動速度加成"), Range(0f, 2f)]
    public float blockMovementMultiplier = 0.5f;
    [Tooltip("完美防禦變成普通防禦的時間"), Range(0f, 1f)]
    public float perfectBlockTime = 0.2f;
    [Tooltip("防禦冷卻時間")]
    public float blockCooldown = 0.5f;

    [Header("PerfectBlockState")]
    [Tooltip("完美防禦的擊退半徑")]
    public float perfectBlockKnockbackRadius = 1f;
    [Tooltip("完美防禦的擊退力道")]
    public float perfectBlockKnockbackForce = 10f;
    [Tooltip("完美防禦的擊退角度")]
    public Vector2 perfectBlockKnockbackAngle = new(1, 2);

    [Header("WallJumpState")]
    [Tooltip("牆壁跳躍速度")]
    public float wallJumpVelocity = 20f;
    [Tooltip("牆壁跳躍持續時間")]
    public float wallJumpTime = 0.4f;
    [Tooltip("牆壁跳躍角度")]
    public Vector2 wallJumpAngle = new(1, 2);

    [Header("InAirState")]
    [Tooltip("跳躍按鍵的容錯時間(玩家在脫離地面的?秒內按下跳躍，還是算是在地面上跳躍)")]
    public float coyoteTime = 0.2f;
    [Tooltip("放開跳躍後減速的程度(長按跳躍會跳比較高)")]
    public float jumpInpusStopYSpeedMultiplier = 0.5f;
    
    [Header("WallSlideState")]
    [Tooltip("牆壁滑落速度")]
    public float wallSlideVelocity = 3f;

    [Header("WallClimbState")]
    [Tooltip("牆壁爬行速度")]
    public bool canWallClimb = false;
    public float wallClimbVelocity = 3f;

    [Header("CrouchState")]
    [Tooltip("蹲下移動速度加成"), Range(0f, 2f)]
    public float crouchMovementMultiplier = 0.5f;
    public float crouchColliderHeight = 0.95f;
    public float standColliderHeight = 1.65f;

    [Header("DashState")]
    public GameObject afterImagePrefab;
    [Tooltip("以45度為單位旋轉")]
    public bool useFixedDegreeAngle = true;
    [Tooltip("冷卻時間")]
    public float dashCooldown = 0.5f;
    [Tooltip("按著Shift的最長時間")]
    public float maxHoldTime = 1f;
    [Tooltip("按著Shift的緩速尺度"), Range(0.1f,1f)]
    public float holdTimeScale = 0.25f;
    [Tooltip("衝刺持續時間")]
    public float dashTime = 0.2f;
    [Tooltip("衝刺速度")]
    public float dashVelocity = 30f;
    [Tooltip("衝刺時的空氣阻力")]
    public float drag = 10f;
    [Tooltip("衝刺結束時, 如果還在的往上飛, 往上的速度會被乘以這個數值")]
    public float dashEndYMultiplier = 0.2f;
    [Tooltip("殘影之間的距離")]
    public float distanceBetweenAfterImages = 0.5f;

    [Header("LedgeClimbState")]
    public float grabCooldown = 0.5f;
    public Vector2 startOffset;
    public Vector2 stopOffset;
}
