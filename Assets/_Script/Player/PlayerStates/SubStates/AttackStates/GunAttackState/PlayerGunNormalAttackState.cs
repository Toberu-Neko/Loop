using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerGunNormalAttackState : PlayerAttackState
{
    private SO_WeaponData_Gun data;

    private bool attackInput;
    private bool canAttack;
    private float lastAttackTime = 0f;

    private int xInput;

    private Vector2 mouseDirectionInput;


    public PlayerGunNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        attackInput = player.InputHandler.AttackInput;
        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;
        xInput = player.InputHandler.NormInputX;

        Movement.SetVelocityX(playerData.movementVelocity * xInput);

        Jump();

        if (mouseDirectionInput.x < 0)
            Movement.CheckIfShouldFlip(-1);
        else if (mouseDirectionInput.x > 0)
            Movement.CheckIfShouldFlip(1);

        if (!attackInput)
        {
            isAttackDone = true;
        }

        /*
        CheckCanAttack();

        if (!attackInput)
        {
            isAttackDone = true;
        }
        else if(canAttack && player.PlayerWeaponManager.GunCurrentEnergy >= data.energyCostPerShot)
        {
            canAttack = false;
            lastAttackTime = Time.time;
            player.PlayerWeaponManager.DecreaseEnergy();
            player.PlayerWeaponManager.GunFired();

            PlayerProjectile proj = GameObject.Instantiate(data.normalAttackObject, player.PlayerWeaponManager.ProjectileStartPos.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            proj.Fire(data.normalAttackDetails, mouseDirectionInput);
        }*/
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        if(player.PlayerWeaponManager.GunCurrentEnergy >= data.energyCostPerShot)
        {
            player.PlayerWeaponManager.DecreaseEnergy();
            player.PlayerWeaponManager.GunFired();

            PlayerProjectile proj = GameObject.Instantiate(data.normalAttackObject, player.PlayerWeaponManager.ProjectileStartPos.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            proj.Fire(data.normalAttackDetails, mouseDirectionInput);
        }
    }

    public void CheckCanAttack()
    {
        if (Time.time >= lastAttackTime + data.attackSpeed)
        {
            canAttack = true;
        }
    }
}
