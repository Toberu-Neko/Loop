using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectileDetails
{
    [Tooltip("�ˮ`")]
    public float damageAmount;
    [Tooltip("�@�O�ˮ`")]
    public float staminaDamageAmount;
    [Tooltip("����t��")]
    public float speed;
    [Tooltip("����ɶ�")]
    public float duration;
    [Tooltip("���h�O�D")]
    public float knockbackStrength;
    [Tooltip("���h����")]
    public Vector2 knockbackAngle;
    
}
