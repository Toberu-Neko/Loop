using System;
using UnityEngine;

[Serializable]
public class S_EnemyStunState
{
    [Tooltip("�w�t����ɶ�")]
    public float stunTime = 1f;
    [Tooltip("�w�t���h�t��(�٭n�[�W�Z�������h, �ҥH���n�դӰ�����n)")]
    public float stunKnockbackSpeed = 20f;
    [Tooltip("�w�t���h����")]
    public Vector2 stunKnockbackAngle;
}

[Serializable]
public class S_EnemyGroundMoveState
{
    [Tooltip("���ʳt��")]
    public float movementSpeed = 3f;
}

[Serializable]
public class S_PlayerDetectedMoveState
{
    [Tooltip("���ʳt��")]
    public float movementSpeed = 5f;
}

[Serializable]
public class S_EnemyPlayerDetectedState
{
    [Tooltip("�����쪱�a��, ����U�@�Ӱʧ@������")]
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
    [Tooltip("���Y����L�����O����Z��")]
    public float projectileTravelDistance;
    */

    [Tooltip("���Y�����Q�R���������ɶ�")]
    public float projectileLifeTime = 8f;
    [Tooltip("�}�b���Y���I���ϰ�j�p")]
    public float rangedAttackRadius = 0.5f;
}

[Serializable]
public class S_EnemyMeleeAttackState
{
    [HideInInspector]public LayerMask whatIsPlayer;

    [Tooltip("�����b�|")]
    public float attackRadius = 0.5f;
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
    [Tooltip("�ਭ����(���a�������줧��, �|�V����?��)")]
    public int amountOfTurns = 2;
    [Tooltip("�ਭ���j�ɶ�")]
    public float timeBetweenTurns = 2f;
}

[Serializable]
public class S_EnemyIdleState
{
    [Tooltip("�̤p���m�ɶ�")]
    public float minIdleTime = 1f;
    [Tooltip("�̤j���m�ɶ�")]
    public float maxIdleTime = 3f;
}

[Serializable]
public class S_EnemyDodgeState
{
    [Tooltip("�{�׳t��")]
    public float dodgeSpeed = 15f;
    [Tooltip("�{�קN�o�ɶ�")]
    public float dodgeCooldown = 3f;
    [Tooltip("�{�ר���")]
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
public class S_EnemyShieldMoveState
{
    [Tooltip("���޲��ʳt��")]
    public float movementSpeed = 1f;
    [Tooltip("��U�޵P�һݮɶ�")]
    public float removeShieldTime = 1f;
}

[Serializable]
public class S_EnemyBookmarkState
{
    public GameObject bookmarkPrefab;
}