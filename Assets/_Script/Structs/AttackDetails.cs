using System;
using UnityEngine;

[Serializable]
public class WeaponAttackDetails
{
    [Tooltip("攻擊名稱, 不填沒關係")]
    public string attackName = "Defult";
    [Tooltip("傷害")]
    public float damageAmount = 10f;
    [Tooltip("耐力傷害")]
    public float staminaDamageAmount = 3f;
    [Tooltip("攻擊時的移動速度, 有些攻擊不會有")]
    public float movementSpeed = 0f;
    [Tooltip("擊退力道")]
    [Range(0, 30)]
    public float knockbackForce = 8f;
    [Tooltip("擊退角度")]
    public Vector2 knockbackAngle;

    public WeaponAttackDetails(float damageAmount, float staminaDamageAmount, Vector2 knockbackAngle, float knockbackForce)
    {
        this.damageAmount = damageAmount;
        this.staminaDamageAmount = staminaDamageAmount;
        this.knockbackForce = knockbackForce;
        this.knockbackAngle = knockbackAngle;
    }
}
