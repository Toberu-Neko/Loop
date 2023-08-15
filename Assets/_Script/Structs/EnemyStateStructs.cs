using System;
using UnityEngine;

[Serializable]
public struct S_EnemyStunState
{
    [Tooltip("暈眩持續時間")]
    public float stunTime;
    [Tooltip("暈眩擊退速度(還要加上武器的擊退, 所以不要調太高比較好)")]
    public float stunKnockbackSpeed;
    [Tooltip("暈眩擊退角度")]
    public Vector2 stunKnockbackAngle;
}

[Serializable]
public struct S_EnemyGroundMoveState
{
    [Tooltip("移動速度")]
    public float movementSpeed;
}

[Serializable]
public struct S_PlayerDetectedMoveState
{
    [Tooltip("移動速度")]
    public float movementSpeed;
}

[Serializable]
public struct S_EnemyPlayerDetectedState
{
    [Tooltip("偵測到玩家後, 執行下一個動作的延遲")]
    public float delayTime;
}

[Serializable]
public struct S_EnemyRangedAttackState
{
    [HideInInspector] public LayerMask whatIsPlayer;

    public GameObject projectile;
    public ProjectileDetails projectileDetails;

    [Tooltip("投擲物件無視重力飛行距離")]
    public float projectileTravelDistance;
    [Tooltip("投擲物件到被刪除為止的持續時間")]
    public float projectileLifeTime;
    [Tooltip("弓箭的頭的碰撞區域大小")]
    public float rangedAttackRadius;
}

[Serializable]
public struct S_EnemyMeleeAttackState
{
    [HideInInspector]public LayerMask whatIsPlayer;

    [Tooltip("攻擊半徑")]
    public float attackRadius;
    [Tooltip("攻擊傷害")]
    public float attackDamage;
    [Tooltip("攻擊耐力傷害")]
    public float staminaAttackDamage;
    [Tooltip("攻擊冷卻時間")]
    public float attackCooldown;

    [Tooltip("擊退角度")]
    public Vector2 knockbackAngle;
    [Tooltip("擊退力道")]
    [Range(8, 50)]
    public float knockbackStrength;
}

[Serializable]
public struct S_EnemySnipingState
{
    public LayerMask whatIsGround;
    public float aimTime;
    public float freazeTime;
    public float reloadTime;
    public AnimationCurve shakeCurve;

    public Gradient aimColor;
    public Gradient lockColor;

    public ProjectileDetails projectileDetails;
}

[Serializable]
public struct S_EnemyLookForPlayerState
{
    [Tooltip("轉身次數(玩家偵測不到之後, 會向後轉?次)")]
    public int amountOfTurns;
    [Tooltip("轉身間隔時間")]
    public float timeBetweenTurns;
}

[Serializable]
public struct S_EnemyIdleState
{
    [Tooltip("最小閒置時間")]
    public float minIdleTime;
    [Tooltip("最大閒置時間")]
    public float maxIdleTime;
}

[Serializable]
public struct S_EnemyDodgeState
{
    [Tooltip("閃避速度")]
    public float dodgeSpeed;
    [Tooltip("閃避冷卻時間")]
    public float dodgeCooldown;
    [Tooltip("閃避角度")]
    public Vector2 dodgeAngle;
}

[Serializable]
public struct S_EnemyDeadState
{
    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;
}

[Serializable]
public struct S_EnemyChargeState
{
    [Tooltip("冷卻時間, 從開始衝刺起算。")]
    public float chargeCooldown;

    [Tooltip("衝刺完的喘氣時間")]
    public float finishChargeDelay;

    [Tooltip("衝刺速度")]
    public float chargeSpeed;

    [Tooltip("衝刺時間長度")]
    public float chargeTime;
}

[Serializable]
public struct S_EnemyShieldMoveState
{
    [Tooltip("持盾移動速度")]
    public float movementSpeed;
    [Tooltip("放下盾牌所需時間")]
    public float removeShieldTime;
}