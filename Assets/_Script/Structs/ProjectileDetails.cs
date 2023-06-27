using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectileDetails
{
    public float damageAmount;
    public float staminaDamageAmount;
    public float speed;
    public float duration;

    public Vector2 knockbackAngle;
    public float knockbackStrength;
    
}
