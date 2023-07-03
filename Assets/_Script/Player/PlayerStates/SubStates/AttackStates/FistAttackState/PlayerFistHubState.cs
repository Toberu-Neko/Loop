using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistHubState : PlayerAttackState
{
    private SO_WeaponData_Fist data;
    private bool canAttack;

    private int xInput;
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

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        Movement.CheckIfShouldFlip(xInput);

        if(chargeStage == 0 && Time.time >= startTime + strongAttackHoldTime)
        {
            chargeStage++;
            lastChargeTime = Time.time;
            player.Anim.SetInteger("fistHubChargeStage", chargeStage);
        }
        if (chargeStage > 0 && Time.time >= lastChargeTime + useSoulTime && chargeStage <= data.maxEnergy && player.PlayerWeaponManager.FistCurrentEnergy > 0) 
        {
            chargeStage++;
            lastChargeTime = Time.time;
            player.PlayerWeaponManager.DecreaseEnergy();
            player.Anim.SetInteger("fistHubChargeStage", chargeStage);
        }

        if (!holdAttackInput)
        {
            switch (chargeStage)
            {
                case 0:
                    Debug.Log("NormalAttack");
                    break;
                case 1:
                    Debug.Log("StrongAttack");
                    break;
                case 2:
                    Debug.Log("SoulOneAttack");
                    break;
                case 3:
                    Debug.Log("SoulTwoAttack");
                    break;
                case 4:
                    Debug.Log("SoulThreeAttack");
                    break;
                case 5:
                    Debug.Log("SoulFourAttack");
                    break;
                case 6:
                    Debug.Log("SoulFiveAttack");
                    break;
                default:
                    Debug.LogError("error at counting fist charge");
                    break;
            }
            isAttackDone = true;
        }


    }

    public bool CheckIfCanAttack() => canAttack;
    public void SetCanAttackFalse() => canAttack = false;
    public void ResetCanAttack() => canAttack = true;
}
