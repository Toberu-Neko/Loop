using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField]
    private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive;
    private float knockbackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float damageAmount)
    {
        Debug.Log(core.transform.parent.name + "Damaged");
        core.Stats.DecreaseHeakth(damageAmount);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        core.Movement.SetVelocity(strength, angle, direction);
        core.Movement.CanSetVelocity = false;

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        // Debug.Log(isKnockbackActive);
        if (isKnockbackActive && ((core.Movement.CurrentVelocity.y <= 0.01f && core.CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            // Debug.Log("Reset");
            isKnockbackActive = false;
            core.Movement.CanSetVelocity = true;
        }
    }
}
