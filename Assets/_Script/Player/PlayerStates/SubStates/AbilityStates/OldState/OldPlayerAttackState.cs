using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int xInput;

    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;

    public OldPlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;
        player.InputHandler.UseAttackInput();

        weapon.EnterWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;

        if (shouldCheckFlip)
        {
            Movement.CheckIfShouldFlip(xInput);
        }

        if (setVelocity)
        {
            Movement.SetVelocityX(velocityToSet * Movement.FacingDirection);
        }
    }


    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        Movement.SetVelocityX(velocity * Movement.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }
    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }
    public Vector2 GetPlayerPosition()
    {
        return (Vector2)core.transform.position;
    }

    #region Animation Triggers
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }
    #endregion
}
