using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunHubState : PlayerGunAttackState
{
    SO_WeaponData_Gun data;

    private int xInput;
    private Vector2 mouseDirectionInput;

    private bool attackInput;
    public PlayerGunHubState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;
        xInput = player.InputHandler.NormInputX;


        Movement.SetVelocityX(playerData.movementVelocity * xInput);

        Jump();

        if (mouseDirectionInput.x < 0)
            Movement.CheckIfShouldFlip(-1);
        else if (mouseDirectionInput.x > 0)
            Movement.CheckIfShouldFlip(1);


        attackInput = player.InputHandler.AttackInput;

        if (!attackInput && Time.time < StartTime + data.minChargeTime)
        {
            stateMachine.ChangeState(player.GunNormalAttackState);
        }
        else if (Time.time >= StartTime + data.minChargeTime)
        {
            stateMachine.ChangeState(player.GunChargeAttackState);
        }
    }
}
