using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjToCombat : MonoBehaviour, IDamageable, IKnockbackable, IStaminaDamageable
{
    [SerializeField] private Combat combat;

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable = true)
    {
        combat.Damage(damageAmount, damagePosition, blockable);
    }

    public void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true, bool forceKnockback = false)
    {
        combat.Knockback(angle, force, damagePosition, blockable);
    }

    public void TakeStaminaDamage(float damageAmount, Vector2 damagePosition, bool blockable = true)
    {
        combat.TakeStaminaDamage(damageAmount, damagePosition, blockable);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
