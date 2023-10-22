using System;
using UnityEngine;

[Serializable]
public class ProjectileDetails
{
    [Tooltip("����ɶ�")]
    public float duration = 5f;
    public float speed = 10f;
    public DamageDetails combatDetails;
}

[Serializable]
public class DamageDetails
{
    [Tooltip("�ˮ`")]
    public float damageAmount = 5f;
    [Tooltip("�@�O�ˮ`")]
    public float staminaDamageAmount = 1f;
    [Tooltip("���h�O�D")]
    [Range(0, 30)]
    public float knockbackStrength = 5f;
    [Tooltip("���h����")]
    public Vector2 knockbackAngle;
    public bool blockable = true;
}
