using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackDetails
{
    public Vector2 position;
    public float damageAmount;
    public float stunDamageAmount;

    public AttackDetails(Vector2 position, float damageAmount, float stunDamageAmount)
    {
        this.position = position;
        this.damageAmount = damageAmount;
        this.stunDamageAmount = stunDamageAmount;
    }
}
