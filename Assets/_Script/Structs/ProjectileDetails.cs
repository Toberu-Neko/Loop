using System;
using UnityEngine;

[Serializable]
public class ProjectileDetails
{
    [Tooltip("持續時間")]
    public float duration = 5f;
    public float speed = 10f;
    public DamageDetails combatDetails;
}

[Serializable]
public class DamageDetails
{
    [Tooltip("傷害")]
    public float damageAmount = 5f;
    [Tooltip("耐力傷害")]
    public float staminaDamageAmount = 1f;
    [Tooltip("擊退力道")]
    [Range(0, 30)]
    public float knockbackStrength = 5f;
    [Tooltip("擊退角度")]
    public Vector2 knockbackAngle;
    public bool blockable = true;
}
