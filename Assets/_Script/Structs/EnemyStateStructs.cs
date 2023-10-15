using System;
using UnityEngine;

[Serializable]
public class ED_EnemyStunState
{
    [Tooltip("暈眩持續時間")]
    public float stunTime = 1f;
    [Tooltip("暈眩擊退速度")]
    public float stunKnockbackSpeed = 0f;
    [Tooltip("暈眩擊退角度")]
    public Vector2 stunKnockbackAngle;
}

[Serializable]
public class ED_EnemyGroundMoveState
{
    [Tooltip("移動速度")]
    public float movementSpeed = 3f;
}

[Serializable]
public class ED_PlayerDetectedMoveState
{
    [Tooltip("移動速度")]
    public float movementSpeed = 5f;

    public float minInStateTime = 3f;
    public float maxInStateTime = 5f;
}

[Serializable]
public class ED_EnemyPlayerDetectedState
{
    [Tooltip("偵測到玩家後, 執行下一個動作的延遲")]
    public float delayTime = 1f;
}

[Serializable]
public class ED_EnemyRangedAttackState
{
    public bool aimPlayer = false;
    public float attackCooldown = 1f;
    public GameObject projectile;
    public ProjectileDetails projectileDetails;
}

[Serializable]
public class ED_EnemyMeleeAttackState
{
    [HideInInspector]public LayerMask whatIsPlayer;

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
public class ED_EnemySnipingState
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
public class ED_EnemyLookForPlayerState
{
    [Tooltip("轉身次數(玩家偵測不到之後, 會向後轉?次)")]
    public int amountOfTurns = 2;
    [Tooltip("轉身間隔時間")]
    public float timeBetweenTurns = 2f;
}

[Serializable]
public class ED_EnemyIdleState
{
    [Tooltip("最小閒置時間")]
    public float minIdleTime = 1f;
    [Tooltip("最大閒置時間")]
    public float maxIdleTime = 3f;
}

[Serializable]
public class ED_EnemyDodgeState
{
    [Tooltip("閃避速度")]
    public float dodgeSpeed = 15f;
    [Tooltip("閃避冷卻時間")]
    public float dodgeCooldown = 3f;
    [Tooltip("閃避角度")]
    public Vector2 dodgeAngle;
}

[Serializable]
public class ED_EnemyDeadState
{
    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;
}

[Serializable]
public class ED_EnemyChargeState
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
public class ED_EnemyShieldMoveState
{
    [Tooltip("持盾移動速度")]
    public float movementSpeed = 1f;
    [Tooltip("放下盾牌所需時間")]
    public float removeShieldTime = 1f;
}

[Serializable]
public class ED_EnemyBookmarkState
{
    public GameObject bookmarkPrefab;
}

[Serializable]
public class ED_EnemyMultiShootState
{
    public float attackCooldown = 5f;
    public float attackVelocity = 10f;
    public ObjContainer[] bullet;

    public class ObjContainer
    {
        public GameObject obj;
        public ProjectileDetails details;
    }
}

[Serializable]
public class ED_EnemyJumpAndMultiAttackState
{
    public Vector2 jumpAngle;
    public float jumpForce = 15f;
    public float attackCooldown = 10f;
    public float attackVelocity = 20f;
    public int attackAmount = 4;
    public ObjContainer[] bullets;

    [Serializable]
    public class ObjContainer
    {
        public GameObject obj;
        public ProjectileDetails details;
    }
}

[Serializable]
public class ED_ChooseRandomBulletState
{
    public int randomCount = 3;
}

[Serializable]
public class ED_EnemyPerfectBlockState
{
    public float cooldown = 10f;
    public float radius = 1.5f;
    public float knockbackForce = 10f;
    public Vector2 knockbackAngle;
}

[Serializable]
public class ED_EnemyProjectiles
{
    public GameObject[] pasteItems;
}

[Serializable]
public class ED_FlyingMovementState
{
    public int minMoveCount = 1;
    public int maxMoveCount = 3;

    public float moveTime = 2f;
    public float movementSpeed = 5f;
}

[Serializable]
public class ED_FourSkyAttackState
{
    public GameObject[] projectileObjs;
    public ProjectileDetails details;

    public float spawnDelay = 0.5f;
    public float fireDelay = 0.5f;
    public float rewindDelay = 0.25f;
    public float attackDistance = 10f;
}

[Serializable]
public class  ED_BackToIdleState
{
    public float stunTime = 3f;
}