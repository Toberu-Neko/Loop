using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistStaticStrongAttackState : PlayerFistAttackState
{
    private SO_WeaponData_Fist data;
    private bool doSoulAttack;
    public PlayerFistStaticStrongAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
    }

    public override void Enter()
    {
        base.Enter();

        doSoulAttack = false;

        if(player.WeaponManager.FistCurrentEnergy > 0)
        {
            doSoulAttack = true;
            player.WeaponManager.DecreaseEnergy();
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        if (doSoulAttack)
        {
            DoDamageToDamageList(WeaponType.Fist, data.staticSoulAttackDetail, false); 
        }
        else
        {
            DoDamageToDamageList(WeaponType.Fist, data.staticStrongAttackDetail);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }
}
