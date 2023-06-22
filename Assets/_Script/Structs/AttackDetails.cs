using System;
using UnityEngine;

[Serializable]
public struct WeaponAttackDetails
{
    public string attackName;
    public float movementSpeed;
    public float damageAmount;

    public float knockbackForce;
    public Vector2 knockbackAngle;
}
