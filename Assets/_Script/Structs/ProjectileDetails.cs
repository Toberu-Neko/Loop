using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProjectileDetails
{
    [Tooltip("傷害")]
    public float damageAmount;
    [Tooltip("耐力傷害")]
    public float staminaDamageAmount;
    [Tooltip("飛行速度")]
    public float speed;
    [Tooltip("持續時間")]
    public float duration;
    [Tooltip("擊退力道")]
    [Range(8, 50)]
    public float knockbackStrength;
    [Tooltip("擊退角度")]
    public Vector2 knockbackAngle;
    
}
