using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerGunChargingState : PlayerAttackState
{
    private bool holdSkillInput;
    private SO_WeaponData_Gun data;
    private GunChargeAttackScript chargeAttack;
    private bool shootable;
    private bool moveable;

    private int xInput;
    public PlayerGunChargingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseWeaponSkillInput();

        shootable = true;
        moveable = true;
    }
    public override void Exit()
    {
        base.Exit();

        player.Anim.SetBool("gunChargeShoot", false);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        xInput = player.InputHandler.NormInputX;
        holdSkillInput = player.InputHandler.WeaponSkillHoldInput;

        if (moveable)
        {
            Movement.CheckIfShouldFlip(xInput);
            Movement.SetVelocityX(playerData.movementVelocity * data.chargeMovementSpeedMultiplier * xInput);
        }

        if(!holdSkillInput && Time.time >= startTime + data.minChargeTime && shootable)
        {
            player.Anim.SetBool("gunChargeShoot", true);
            shootable = false;
            moveable = false;
        }
    }
    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(data.chargeAttackBackFireVelocity * -Movement.FacingDirection);
    }
    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityX(0f);
        GameObject.Destroy(chargeAttack.gameObject);
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        float chargeTime = Time.time - startTime;
        chargeAttack = GameObject.Instantiate(data.chargeAttackObject, player.PlayerWeaponManager.ProjectileStartPos.position, player.transform.rotation, player.transform).GetComponent<GunChargeAttackScript>();
        chargeAttack.Init(new Vector2(chargeTime * data.chargeAttackWidthPerSecond, data.chargeAttackHeight), player.transform.eulerAngles);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }
}
