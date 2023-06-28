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
    private bool jumpInput;
    private bool jumpInputStop;
    private Vector2 mouseDirectionInput;

    private bool isJumping;

    public PlayerGunNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        isJumping = false;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        attackInput = player.InputHandler.AttackInput;
        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;

        MovementAndJump();

        CheckCanAttack();

        if (!attackInput)
        {
            isAbilityDone = true;
        }
        else if(canAttack && player.PlayerWeaponManager.GunCurrentEnergy >= data.energyCostPerShot)
        {
            canAttack = false;
            lastAttackTime = Time.time;
            player.PlayerWeaponManager.DecreaseEnergy();
            player.PlayerWeaponManager.GunFired();

            PlayerProjectile proj = GameObject.Instantiate(data.normalAttackObject, player.PlayerWeaponManager.ProjectileStartPos.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            proj.Fire(data.normalAttackDetails, mouseDirectionInput);
        }
    }

    private void MovementAndJump()
    {
        Movement.SetVelocityX(playerData.movementVelocity * xInput);

        CheckJumpMultiplier();

        if (jumpInput && player.JumpState.AmountOfJumpsLeft > 0)
        {
            Movement.SetVelocityY(playerData.jumpVelocity);
            player.JumpState.DecreaseAmountOfJumpsLeft();
            player.InputHandler.UseJumpInput();
            isJumping = true;
        }

        if (isGrounded && Movement.CurrentVelocity.y < 0.01f && player.JumpState.AmountOfJumpsLeft != playerData.amountOfJumps)
        {
            player.JumpState.ResetAmountOfJumpsLeft();
        }

        if (mouseDirectionInput.x < 0)
            Movement.CheckIfShouldFlip(-1);
        else if (mouseDirectionInput.x > 0)
            Movement.CheckIfShouldFlip(1);
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
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
