using System;
using UnityEngine;

[Serializable]
public class ED_EnemyStunState
{
    [Tooltip("�w�t����ɶ�")]
    public float stunTime = 1f;
    [Tooltip("�w�t���h�t��")]
    public float stunKnockbackSpeed = 0f;
    [Tooltip("�w�t���h����")]
    public Vector2 stunKnockbackAngle;
}

[Serializable]
public class ED_EnemyGroundMoveState
{
    [Tooltip("���ʳt��")]
    public float movementSpeed = 3f;
}

[Serializable]
public class ED_PlayerDetectedMoveState
{
    [Tooltip("���ʳt��")]
    public float movementSpeed = 5f;

    public float minInStateTime = 3f;
    public float maxInStateTime = 5f;
}

[Serializable]
public class ED_EnemyPlayerDetectedState
{
    [Tooltip("�����쪱�a��, ����U�@�Ӱʧ@������")]
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

    [Tooltip("�����ˮ`")]
    public float attackDamage = 10f;
    [Tooltip("�����@�O�ˮ`")]
    public float staminaAttackDamage = 3f;
    [Tooltip("�����N�o�ɶ�")]
    public float attackCooldown = 1f;

    [Tooltip("���h����")]
    public Vector2 knockbackAngle;
    [Tooltip("���h�O�D")]
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
    [Tooltip("�ਭ����(���a�������줧��, �|�V����?��)")]
    public int amountOfTurns = 2;
    [Tooltip("�ਭ���j�ɶ�")]
    public float timeBetweenTurns = 2f;
}

[Serializable]
public class ED_EnemyIdleState
{
    [Tooltip("�̤p���m�ɶ�")]
    public float minIdleTime = 1f;
    [Tooltip("�̤j���m�ɶ�")]
    public float maxIdleTime = 3f;
}

[Serializable]
public class ED_EnemyDodgeState
{
    [Tooltip("�{�׳t��")]
    public float dodgeSpeed = 15f;
    [Tooltip("�{�קN�o�ɶ�")]
    public float dodgeCooldown = 3f;
    [Tooltip("�{�ר���")]
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
    [Tooltip("�N�o�ɶ�, �q�}�l�Ĩ�_��C")]
    public float chargeCooldown = 2f;

    [Tooltip("�Ĩ맹���ݮ�ɶ�")]
    public float finishChargeDelay = 1f;

    [Tooltip("�Ĩ�t��")]
    public float chargeSpeed = 6f;

    [Tooltip("�Ĩ�ɶ�����")]
    public float chargeTime = 2f;
}

[Serializable]
public class ED_EnemyShieldMoveState
{
    [Tooltip("���޲��ʳt��")]
    public float movementSpeed = 1f;
    [Tooltip("��U�޵P�һݮɶ�")]
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