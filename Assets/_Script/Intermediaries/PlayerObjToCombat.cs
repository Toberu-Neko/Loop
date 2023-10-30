using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjToCombat : MonoBehaviour, IDamageable, IKnockbackable, IStaminaDamageable, ISlowable
{
    [SerializeField] private Combat combat;

    public void Damage(float damageAmount, Vector2 damagePosition, bool blockable = true)
    {
        combat.Damage(damageAmount, damagePosition, blockable);
    }

    public void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        combat.Knockback(angle, force, damagePosition, blockable);
    }

    public void TakeStaminaDamage(float damageAmount, Vector2 damagePosition, bool blockable = true)
    {
        combat.TakeStaminaDamage(damageAmount, damagePosition, blockable);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void GotoKinematicState(float time)
    {
        Debug.LogError("Player don't have kinematic state!");
    }

    public void GoToStunState()
    {
        Debug.LogError("Player don't have kinematic state!");
    }

    public void SetDebuffMultiplier(float multiplier, float delayTime = 0)
    {
        combat.SetDebuffMultiplier(multiplier, delayTime);
    }

    public void SetDebuffMultiplierOne()
    {
        combat.SetDebuffMultiplierOne();
    }
}
