using System;
using UnityEngine;

[Serializable]
public struct WeaponAttackDetails
{
    [Tooltip("攻擊名稱, 不填沒關係")]
    public string attackName;
    [Tooltip("傷害")]
    public float damageAmount;
    [Tooltip("耐力傷害")]
    public float staminaDamageAmount;
    [Tooltip("攻擊時的移動速度, 有些攻擊不會有")]
    public float movementSpeed;
    [Tooltip("擊退力道")]
    public float knockbackForce;
    [Tooltip("擊退角度")]
    public Vector2 knockbackAngle;
}
