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
    [Tooltip("���ʳt��")]
    public float movementVelocity = 10f;

    [Header("JumpState")]
    [Tooltip("���D����")]
    public float jumpVelocity = 15f;
    [Tooltip("���D�ƶq")]
    public int amountOfJumps = 1;
    public float maxJumpTime = 0.25f;

    [Header("BlockState")]
    [Tooltip("���m���ʳt�ץ[��"), Range(0f, 2f)]
    public float blockMovementMultiplier = 0.5f;
    [Tooltip("�������m�ܦ����q���m���ɶ�"), Range(0f, 1f)]
    public float perfectBlockTime = 0.2f;
    [Tooltip("���m�N�o�ɶ�")]
    public float blockCooldown = 0.5f;

    [Header("PerfectBlockState")]
    [Tooltip("�������m�����h�b�|")]
    public float perfectBlockKnockbackRadius = 1f;
    [Tooltip("�������m�����h�O�D")]
    public float perfectBlockKnockbackForce = 10f;
    [Tooltip("�������m�����h����")]
    public Vector2 perfectBlockKnockbackAngle = new(1, 2);

    [Header("WallJumpState")]
    [Tooltip("������D�t��")]
    public float wallJumpVelocity = 20f;
    [Tooltip("������D����ɶ�")]
    public float wallJumpTime = 0.4f;
    [Tooltip("������D����")]
    public Vector2 wallJumpAngle = new(1, 2);

    [Header("InAirState")]
    [Tooltip("���D���䪺�e���ɶ�(���a�b�����a����?�����U���D�A�٬O��O�b�a���W���D)")]
    public float coyoteTime = 0.2f;
    [Tooltip("��}���D���t���{��(�������D�|�������)")]
    public float jumpInpusStopYSpeedMultiplier = 0.5f;
    
    [Header("WallSlideState")]
    [Tooltip("����Ƹ��t��")]
    public float wallSlideVelocity = 3f;

    [Header("WallClimbState")]
    [Tooltip("�������t��")]
    public bool canWallClimb = false;
    public float wallClimbVelocity = 3f;

    [Header("CrouchState")]
    [Tooltip("�ۤU���ʳt�ץ[��"), Range(0f, 2f)]
    public float crouchMovementMultiplier = 0.5f;
    public float crouchColliderHeight = 0.95f;
    public float standColliderHeight = 1.65f;

    [Header("DashState")]
    public GameObject afterImagePrefab;
    [Tooltip("�H45�׬�������")]
    public bool useFixedDegreeAngle = true;
    [Tooltip("�N�o�ɶ�")]
    public float dashCooldown = 0.5f;
    [Tooltip("����Shift���̪��ɶ�")]
    public float maxHoldTime = 1f;
    [Tooltip("����Shift���w�t�ث�"), Range(0.1f,1f)]
    public float holdTimeScale = 0.25f;
    [Tooltip("�Ĩ����ɶ�")]
    public float dashTime = 0.2f;
    [Tooltip("�Ĩ�t��")]
    public float dashVelocity = 30f;
    [Tooltip("�Ĩ�ɪ��Ů���O")]
    public float drag = 10f;
    [Tooltip("�Ĩ뵲����, �p�G�٦b�����W��, ���W���t�׷|�Q���H�o�Ӽƭ�")]
    public float dashEndYMultiplier = 0.2f;
    [Tooltip("�ݼv�������Z��")]
    public float distanceBetweenAfterImages = 0.5f;

    [Header("LedgeClimbState")]
    public float grabCooldown = 0.5f;
    public Vector2 startOffset;
    public Vector2 stopOffset;
}
