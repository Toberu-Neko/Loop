using System;
using UnityEngine;

[Serializable]
public struct S_EnemyStunState
{
    public float stunTime;
    public float stunKnockbackTime;
    public float stunKnockbackSpeed;

    public Vector2 stunKnockbackAngle;
}

[Serializable]
public struct S_EnemyGroundMoveState
{
    public float movementSpeed;
}

[Serializable]
public struct S_EnemyPlayerDetectedState
{
    public float delayTime;
}

[Serializable]
public struct S_EnemyRangedAttackState
{
    [HideInInspector] public LayerMask whatIsPlayer;
    public GameObject projectile;
    public ProjectileDetails projectileDetails;

    public float projectileTravelDistance;
    public float projectileLifeTime;
    public float projectileGravityScale;
    public float timeBetweenProjectiles;
    public float rangedAttackRadius;
}

[Serializable]
public struct S_EnemyMeleeAttackState
{
    [HideInInspector]public LayerMask whatIsPlayer;

    public float attackRadius;
    public float attackDamage;
    public float staminaAttackDamage;

    public float attackCooldown;

    public Vector2 knockbackAngle;
    public float knockbackStrength;
}

[Serializable]
public struct S_EnemyLookForPlayerState
{
    public int amountOfTurns;
    public float timeBetweenTurns;
}

[Serializable]
public struct S_EnemyIdleState
{
    public float minIdleTime;
    public float maxIdleTime;
}

[Serializable]
public struct S_EnemyDodgeState
{
    public float dodgeSpeed;
    public float dodgeTime;
    public float dodgeCooldown;

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

    public float chargeSpeed;
    [Tooltip("衝刺時間長度")]
    public float chargeTime;
}

[Serializable]
public struct S_EnemyShieldMoveState
{
    public float movementSpeed;
    public float removeShieldTime;
}