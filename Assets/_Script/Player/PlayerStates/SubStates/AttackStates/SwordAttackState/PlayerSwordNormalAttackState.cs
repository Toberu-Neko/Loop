using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordNormalAttackState : PlayerAttackState
{
    private int attackCounter;
    private SO_WeaponData_Sword swordData;
    private WeaponAttackDetails details;

    private float lastAttackTime;


    public PlayerSwordNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        swordData = player.PlayerWeaponManager.SwordData;
        attackCounter = 0;
        lastAttackTime = 0;
    }
    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += () => isAttackDone = true;

        if (Time.time >= lastAttackTime + swordData.resetAttackTime)
        {
            attackCounter = 0;
        }

        details = swordData.NormalAttackDetails[attackCounter];
        player.Anim.SetInteger("attackCount", attackCounter);
    }
    public override void Exit()
    {
        base.Exit();


        Combat.OnDamaged -= () => isAttackDone = true;
        attackCounter++;
        lastAttackTime = Time.time;

        if (attackCounter >= swordData.AmountOfAttacks)
        {
            attackCounter = 0;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement)
        {
            Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
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


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }

}
