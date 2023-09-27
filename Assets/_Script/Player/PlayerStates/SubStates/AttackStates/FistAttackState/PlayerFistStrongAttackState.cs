using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistStrongAttackState : PlayerFistAttackState
{
    private SO_WeaponData_Fist data;

    private bool doAttack;
    private bool startMovement;
    private bool doSoulAttack;
    public PlayerFistStrongAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
    }
    public override void Enter()
    {
        base.Enter();

        doSoulAttack = false;

        if(player.WeaponManager.FistCurrentEnergy > 0)
        {
            player.WeaponManager.DecreaseEnergy();
            doSoulAttack = true;
            Stats.SetInvincibleTrue();
        }
        else
        {
            Combat.OnDamaged += () => isAttackDone = true;
        }

        doAttack = false;
        Debug.Log(doSoulAttack);
    }

    public override void Exit()
    {
        base.Exit();

        if(!doSoulAttack)
        {
            Combat.OnDamaged -= () => isAttackDone = true;
        }
        else
        {
            Stats.SetInvincibleFalse();
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement && Combat.DetectedDamageables.Count == 0 && !doSoulAttack)
        {
            Movement.SetVelocityX(data.strongAttackDetail.movementSpeed * Movement.FacingDirection);
        }
        else if(isAnimationStartMovement && Combat.DetectedDamageables.Count == 0 && doSoulAttack)
        {
            Movement.SetVelocityX(data.soulAttackDetail.movementSpeed * Movement.FacingDirection);
        }
        else
        {
            Movement.SetVelocityX(0f);
        }

        if (doAttack)
        {
            if(!doSoulAttack)
                DoDamageToDamageList(WeaponType.Fist, data.strongAttackDetail.damageAmount, data.strongAttackDetail.staminaDamageAmount, data.strongAttackDetail.knockbackAngle, data.strongAttackDetail.knockbackForce);
            else
                DoDamageToDamageList(WeaponType.Fist, data.soulAttackDetail.damageAmount, data.soulAttackDetail.staminaDamageAmount, data.soulAttackDetail.knockbackAngle, data.soulAttackDetail.knockbackForce, false);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        doAttack = true;
    }
    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        doAttack = false;
        // Combat.Knockback(Vector2.one, 10f, -Movement.FacingDirection, Vector2.zero, false, true);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }
}
