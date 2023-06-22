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
    public float longRangeActionTime;
}

[Serializable]
public struct S_EnemyRangedAttackState
{
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
    public float meleeAttackRadius;
    public float meleeAttackDamage;

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
    public float chargeSpeed;
    public float chargeTime;
}

[Serializable]
public struct E_IdleDataStruct
{
    public float minIdleTime;
    public float maxIdleTime;
}