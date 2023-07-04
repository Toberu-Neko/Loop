using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistNormalAttackState : PlayerAttackState
{
    private int attackCounter;
    private SO_WeaponData_Fist fistData;
    private WeaponAttackDetails details;

    private float lastAttackTime;

    public PlayerFistNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        fistData = player.PlayerWeaponManager.FistData;
        attackCounter = 0;
        lastAttackTime = 0;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += () => isAttackDone = true;

        if (Time.time >= lastAttackTime + fistData.resetAttackTime)
        {
            attackCounter = 0;
        }

        details = fistData.NormalAttackDetails[attackCounter];
        player.Anim.SetInteger("attackCount", attackCounter);
    }

    public override void Exit()
    {
        base.Exit();


        Combat.OnDamaged -= () => isAttackDone = true;
        attackCounter++;
        lastAttackTime = Time.time;

        if (attackCounter >= fistData.AmountOfAttacks)
        {
            attackCounter = 0;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce);
    }
    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }
}
