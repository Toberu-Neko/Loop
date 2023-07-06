using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistHubState : PlayerAttackState
{
    private SO_WeaponData_Fist data;
    private bool canAttack;

    private int xInput;
    private int yInput;
    private bool holdAttackInput;

    private float strongAttackHoldTime;
    private float useSoulTime;

    private int chargeStage;
    private float lastChargeTime;
    
    public PlayerFistHubState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.FistData;
        strongAttackHoldTime = data.strongAttackHoldTime;
        useSoulTime = data.everySoulAddtionalHoldTime;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += () => isAttackDone = true;
        player.InputHandler.UseAttackInput();
        canAttack = false;
        chargeStage = 0;
        lastChargeTime = 0;
        player.Anim.SetInteger("fistHubChargeStage", chargeStage);
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= () => isAttackDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        holdAttackInput = player.InputHandler.HoldAttackInput;
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        Movement.CheckIfShouldFlip(xInput);

        if(chargeStage == 0 && Time.time >= startTime + strongAttackHoldTime)
        {
            chargeStage = 1;
            lastChargeTime = Time.time;
            player.Anim.SetInteger("fistHubChargeStage", chargeStage);
        }
        if (chargeStage == 1 && Time.time >= lastChargeTime + useSoulTime && player.PlayerWeaponManager.FistCurrentEnergy >= 2)
        {
            chargeStage = 2;
            player.PlayerWeaponManager.DecreaseEnergy();
            player.PlayerWeaponManager.DecreaseEnergy(); 
            lastChargeTime = Time.time;
            player.Anim.SetInteger("fistHubChargeStage", chargeStage);
        }
        if (chargeStage >= 2 && Time.time >= lastChargeTime + useSoulTime && chargeStage < data.maxEnergy && player.PlayerWeaponManager.FistCurrentEnergy > 0) 
        {
            chargeStage++;
            lastChargeTime = Time.time;
            player.PlayerWeaponManager.DecreaseEnergy();
            player.Anim.SetInteger("fistHubChargeStage", chargeStage);
        }

        if(yInput < 0)
        {
            switch (chargeStage)
            {
                case 0:
                    stateMachine.ChangeState(player.FistNormalAttackState);
                    break;
                case 1:// Strong Attack
                case 2:// C2
                case 3:// C3
                case 4:// C4
                case 5:// C5
                    player.FistSoulAttackState.SetStaticAttack(true);
                    player.FistSoulAttackState.SetSoulAmount(chargeStage - 1);
                    stateMachine.ChangeState(player.FistSoulAttackState);
                    break;

                default:
                    Debug.LogError("error at counting fist charge");
                    isAttackDone = true;
                    break;
            }
        }
        if (!holdAttackInput)
        {
            switch (chargeStage)
            {
                case 0:
                    stateMachine.ChangeState(player.FistNormalAttackState);
                    break;
                case 1:// Strong Attack
                case 2:// C2
                case 3:// C3
                case 4:// C4
                case 5:// C5
                    player.FistSoulAttackState.SetStaticAttack(false);
                    player.FistSoulAttackState.SetSoulAmount(chargeStage - 1);
                    stateMachine.ChangeState(player.FistSoulAttackState);
                    break;

                default:
                    Debug.LogError("error at counting fist charge");
                    isAttackDone = true;
                    break;
            }
        }


    }

    public bool CheckIfCanAttack() => canAttack;
    public void SetCanAttackFalse() => canAttack = false;
    public void ResetCanAttack() => canAttack = true;
}
