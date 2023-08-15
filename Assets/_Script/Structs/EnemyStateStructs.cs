using System;
using UnityEngine;

[Serializable]
public struct S_EnemyStunState
{
    [Tooltip("�w�t����ɶ�")]
    public float stunTime;
    [Tooltip("�w�t���h�t��(�٭n�[�W�Z�������h, �ҥH���n�դӰ�����n)")]
    public float stunKnockbackSpeed;
    [Tooltip("�w�t���h����")]
    public Vector2 stunKnockbackAngle;
}

[Serializable]
public struct S_EnemyGroundMoveState
{
    [Tooltip("���ʳt��")]
    public float movementSpeed;
}

[Serializable]
public struct S_PlayerDetectedMoveState
{
    [Tooltip("���ʳt��")]
    public float movementSpeed;
}

[Serializable]
public struct S_EnemyPlayerDetectedState
{
    [Tooltip("�����쪱�a��, ����U�@�Ӱʧ@������")]
    public float delayTime;
}

[Serializable]
public struct S_EnemyRangedAttackState
{
    [HideInInspector] public LayerMask whatIsPlayer;

    public GameObject projectile;
    public ProjectileDetails projectileDetails;

    [Tooltip("���Y����L�����O����Z��")]
    public float projectileTravelDistance;
    [Tooltip("���Y�����Q�R���������ɶ�")]
    public float projectileLifeTime;
    [Tooltip("�}�b���Y���I���ϰ�j�p")]
    public float rangedAttackRadius;
}

[Serializable]
public struct S_EnemyMeleeAttackState
{
    [HideInInspector]public LayerMask whatIsPlayer;

    [Tooltip("�����b�|")]
    public float attackRadius;
    [Tooltip("�����ˮ`")]
    public float attackDamage;
    [Tooltip("�����@�O�ˮ`")]
    public float staminaAttackDamage;
    [Tooltip("�����N�o�ɶ�")]
    public float attackCooldown;

    [Tooltip("���h����")]
    public Vector2 knockbackAngle;
    [Tooltip("���h�O�D")]
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
    [Tooltip("�ਭ����(���a�������줧��, �|�V����?��)")]
    public int amountOfTurns;
    [Tooltip("�ਭ���j�ɶ�")]
    public float timeBetweenTurns;
}

[Serializable]
public struct S_EnemyIdleState
{
    [Tooltip("�̤p���m�ɶ�")]
    public float minIdleTime;
    [Tooltip("�̤j���m�ɶ�")]
    public float maxIdleTime;
}

[Serializable]
public struct S_EnemyDodgeState
{
    [Tooltip("�{�׳t��")]
    public float dodgeSpeed;
    [Tooltip("�{�קN�o�ɶ�")]
    public float dodgeCooldown;
    [Tooltip("�{�ר���")]
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
    [Tooltip("�N�o�ɶ�, �q�}�l�Ĩ�_��C")]
    public float chargeCooldown;

    [Tooltip("�Ĩ맹���ݮ�ɶ�")]
    public float finishChargeDelay;

    [Tooltip("�Ĩ�t��")]
    public float chargeSpeed;

    [Tooltip("�Ĩ�ɶ�����")]
    public float chargeTime;
}

[Serializable]
public struct S_EnemyShieldMoveState
{
    [Tooltip("���޲��ʳt��")]
    public float movementSpeed;
    [Tooltip("��U�޵P�һݮɶ�")]
    public float removeShieldTime;
}