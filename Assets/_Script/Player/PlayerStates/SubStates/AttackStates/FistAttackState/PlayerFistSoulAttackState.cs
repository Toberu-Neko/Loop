using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistSoulAttackState : PlayerAttackState
{
    private int soulAmount;
    private bool staticAttack;
    private SO_WeaponData_Fist data;
    private WeaponAttackDetails details;

    private bool doAttack;
    public PlayerFistSoulAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.FistData;
        soulAmount = 0;
    }
    public override void Enter()
    {
        base.Enter();

        details = data.soulAttackDetails[soulAmount];
        player.Anim.SetBool("fistStaticAttack", staticAttack);
        player.Anim.SetInteger("attackCount", soulAmount);
        doAttack = false;

        if(soulAmount == 0)
        {
            Combat.OnDamaged += () => isAttackDone = true;
        }
        else
        {
            Stats.SetInvincibleTrue();
        }
    }

    public override void Exit()
    {
        base.Exit();

        if(soulAmount == 0)
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

        if (doAttack)
        {
            if(soulAmount == 0)
                DoDamageToDamageList(details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce);
            else
                DoDamageToDamageList(details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce, false);
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

        Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);

    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
        doAttack = false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }


    public void SetSoulAmount(int soulAmount) => this.soulAmount = soulAmount;
    public void SetStaticAttack(bool staticAttack) => this.staticAttack = staticAttack;
}
