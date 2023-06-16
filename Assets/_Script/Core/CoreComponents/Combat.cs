using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;
    private CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>(); }
    private CollisionSenses collisionSenses;

    private Stats Stats { get => stats ??= core.GetComponent<Stats>();}
    private Stats stats;

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
        Stats?.DecreaseHeakth(damageAmount);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        // Debug.Log(isKnockbackActive);
        if (isKnockbackActive && ((Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            // Debug.Log("Reset");
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }
}
