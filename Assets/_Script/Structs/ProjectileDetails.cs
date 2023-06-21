using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectileDetails
{
    public float damageAmount;
    public float speed;
    public float duration;
    [HideInInspector] public int facingDirection;
    public Vector2 knockbackAngle;
    public float knockbackStrength;
    
}
