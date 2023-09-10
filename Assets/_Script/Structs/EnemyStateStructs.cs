using System;
using UnityEngine;

[Serializable]
public class S_EnemyStunState
{
    [Tooltip("暈眩持續時間")]
    public float stunTime = 1f;
    [Tooltip("暈眩擊退速度(還要加上武器的擊退, 所以不要調太高比較好)")]
    public float stunKnockbackSpeed = 20f;
    [Tooltip("暈眩擊退角度")]
    public Vector2 stunKnockbackAngle;
}

[Serializable]
public class S_EnemyGroundMoveState
{
    [Tooltip("移動速度")]
    public float movementSpeed = 3f;
}

[Serializable]
public class S_PlayerDetectedMoveState
{
    [Tooltip("移動速度")]
    public float movementSpeed = 5f;
}

[Serializable]
public class S_EnemyPlayerDetectedState
{
    [Tooltip("偵測到玩家後, 執行下一個動作的延遲")]
    public float delayTime = 1f;
}

[Serializable]
public class S_EnemyRangedAttackState
{
    [HideInInspector] public LayerMask whatIsPlayer;

    public bool aimPlayer = false;
    public float attackCooldown = 1f;
    public GameObject projectile;
    public ProjectileDetails projectileDetails;

    /*
    [Tooltip("投擲物件無視重力飛行距離")]
    public float projectileTravelDistance;
    */

    [Tooltip("投擲物件到被刪除為止的持續時間")]
    public float projectileLifeTime = 8f;
    [Tooltip("弓箭的頭的碰撞區域大小")]
    public float rangedAttackRadius = 0.5f;
}

[Serializable]
public class S_EnemyMeleeAttackState
{
    [HideInInspector]public LayerMask whatIsPlayer;

    [Tooltip("攻擊半徑")]
    public float attackRadius = 0.5f;
    [Tooltip("攻擊傷害")]
    public float attackDamage = 10f;
    [Tooltip("攻擊耐力傷害")]
    public float staminaAttackDamage = 3f;
    [Tooltip("攻擊冷卻時間")]
    public float attackCooldown = 1f;

    [Tooltip("擊退角度")]
    public Vector2 knockbackAngle;
    [Tooltip("擊退力道")]
    [Range(8, 50)]
    public float knockbackStrength = 10f;
}

[Serializable]
public class S_EnemySnipingState
{
    public LayerMask whatIsGround;
    public GameObject bulletPrefab;

    public float cooldown = 0f;
    public float aimTime = 1.2f;
    public float freazeTime = 0.8f;
    public float reloadTime = 2f;
    public AnimationCurve shakeCurve;

    public Gradient aimColor;
    public Gradient lockColor;

    public ProjectileDetails bulletDetails;

}

[Serializable]
public class S_EnemyLookForPlayerState
{
    [Tooltip("轉身次數(玩家偵測不到之後, 會向後轉?次)")]
    public int amountOfTurns = 2;
    [Tooltip("轉身間隔時間")]
    public float timeBetweenTurns = 2f;
}

[Serializable]
public class S_EnemyIdleState
{
    [Tooltip("最小閒置時間")]
    public float minIdleTime = 1f;
    [Tooltip("最大閒置時間")]
    public float maxIdleTime = 3f;
}

[Serializable]
public class S_EnemyDodgeState
{
    [Tooltip("閃避速度")]
    public float dodgeSpeed = 15f;
    [Tooltip("閃避冷卻時間")]
    public float dodgeCooldown = 3f;
    [Tooltip("閃避角度")]
    public Vector2 dodgeAngle;
}

[Serializable]
public class S_EnemyDeadState
{
    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;
}

[Serializable]
public class S_EnemyChargeState
{
    [Tooltip("冷卻時間, 從開始衝刺起算。")]
    public float chargeCooldown = 2f;

    [Tooltip("衝刺完的喘氣時間")]
    public float finishChargeDelay = 1f;

    [Tooltip("衝刺速度")]
    public float chargeSpeed = 6f;

    [Tooltip("衝刺時間長度")]
    public float chargeTime = 2f;
}

[Serializable]
public class S_EnemyShieldMoveState
{
    [Tooltip("持盾移動速度")]
    public float movementSpeed = 1f;
    [Tooltip("放下盾牌所需時間")]
    public float removeShieldTime = 1f;
}

[Serializable]
public class S_EnemyBookmarkState
{
    public GameObject bookmarkPrefab;
}