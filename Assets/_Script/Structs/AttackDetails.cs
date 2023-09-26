using System;
using UnityEngine;

[Serializable]
public class WeaponAttackDetails
{
    [Tooltip("�����W��, ����S���Y")]
    public string attackName = "Defult";
    [Tooltip("�ˮ`")]
    public float damageAmount = 10f;
    [Tooltip("�@�O�ˮ`")]
    public float staminaDamageAmount = 3f;
    [Tooltip("�����ɪ����ʳt��, ���ǧ������|��")]
    public float movementSpeed = 0f;
    [Tooltip("���h�O�D")]
    [Range(0, 30)]
    public float knockbackForce = 8f;
    [Tooltip("���h����")]
    public Vector2 knockbackAngle;
}
