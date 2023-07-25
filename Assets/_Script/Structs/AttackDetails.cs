using System;
using UnityEngine;

[Serializable]
public struct WeaponAttackDetails
{
    [Tooltip("�����W��, ����S���Y")]
    public string attackName;
    [Tooltip("�ˮ`")]
    public float damageAmount;
    [Tooltip("�@�O�ˮ`")]
    public float staminaDamageAmount;
    [Tooltip("�����ɪ����ʳt��, ���ǧ������|��")]
    public float movementSpeed;
    [Tooltip("���h�O�D")]
    [Range(8, 50)]
    public float knockbackForce;
    [Tooltip("���h����")]
    public Vector2 knockbackAngle;
}
