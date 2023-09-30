using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistS3AttackState : PlayerFistAttackState
{
    private SO_WeaponData_Fist data;
    private WeaponAttackDetails[] details;
    private GameObject target;
    private IDamageable damageable;
    private int count = 0;
    public PlayerFistS3AttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
        details = data.soulThreeAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        count = 0;
        Movement.SetRBKinematic();
        Stats.SetInvincibleTrue();

        Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
        Stats.SetInvincibleFalse();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement && !CollisionSenses.SolidCeiling)
        {
            Movement.SetVelocityY(data.s3GoUpVelocity);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Fist,details[count]);
        count++;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        damageable.GoToStunState();
        target.transform.SetParent(null);
        isAttackDone = true;
    }

    public void SetTargetObj(GameObject target, IDamageable dam)
    {
        this.target = target;
        damageable = dam;
    }

}
