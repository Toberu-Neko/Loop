using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    protected bool isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
            isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAbilityDone)
        {
            if(isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Combat.DetectedDamageables.Clear();
        Combat.DetectedKnockbackables.Clear();
        Combat.DetectedStaminaDamageables.Clear();
        Combat.DetectedMapDamageableItems.Clear();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void DoDamageToDamageList(float damageAmount,float damageStaminaAmount ,Vector2 knockBackAngle, float knockBackForce, bool blockable = true)
    {
        if (Combat.DetectedDamageables.Count > 0)
        {
            foreach (IDamageable damageable in Combat.DetectedDamageables.ToList())
            {
                damageable.Damage(damageAmount, core.transform.position, blockable);
            }
        }

        if (Combat.DetectedKnockbackables.Count > 0)
        {
            foreach (IKnockbackable knockbackable in Combat.DetectedKnockbackables.ToList())
            {
                knockbackable.Knockback(knockBackAngle, knockBackForce, Movement.ParentTransform.position, blockable);
            }
        }

        if(Combat.DetectedStaminaDamageables.Count > 0)
        {
            foreach (IStaminaDamageable staminaDamageable in Combat.DetectedStaminaDamageables.ToList())
            {
                staminaDamageable.TakeStaminaDamage(damageStaminaAmount, core.transform.position, blockable);
            }
        }

        if(Combat.DetectedMapDamageableItems.Count > 0)
        {
            foreach (IMapDamageableItem mapDamageableItem in Combat.DetectedMapDamageableItems.ToList())
            {
                mapDamageableItem.TakeDamage(damageAmount);
            }
        }
    }


}
