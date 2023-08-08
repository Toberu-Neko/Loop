using System;
using System.Linq;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected bool isAttackDone;

    protected bool isGrounded;

    private bool jumpInput;
    private bool jumpInputStop;
    private bool isJumping;

    private event Action OnAttack;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
            isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();
        Stats.SetCanChangeWeapon(false);
        isAttackDone = false;
        isJumping = false;
        OnAttack += Combat.HandleOnAttack;
        OnAttack += player.TimeSkillManager.HandleOnAttack;
    }

    public override void Exit()
    {
        base.Exit();
        Stats.SetCanChangeWeapon(true);
        OnAttack -= Combat.HandleOnAttack;
        OnAttack -= player.TimeSkillManager.HandleOnAttack;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;

        if (isAttackDone)
        {
            if(isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();


        Combat.DetectedDamageables.Clear();
        Combat.DetectedKnockbackables.Clear();
        Combat.DetectedStaminaDamageables.Clear();
    }

    public void DoDamageToDamageList(float damageAmount,float damageStaminaAmount ,Vector2 knockBackAngle, float knockBackForce, bool blockable = true)
    {
        if (Combat.DetectedDamageables.Count > 0)
        {
            OnAttack?.Invoke();
            foreach (IDamageable damageable in Combat.DetectedDamageables.ToList())
            {
                damageable.Damage(damageAmount, core.transform.position, blockable);
            }
        }

        if (Combat.DetectedKnockbackables.Count > 0)
        {
            foreach (IKnockbackable knockbackable in Combat.DetectedKnockbackables.ToList())
            {
                knockbackable.Knockback(knockBackAngle, knockBackForce, Movement.FacingDirection, (Vector2)core.transform.position, blockable);
            }
        }

        if(Combat.DetectedStaminaDamageables.Count > 0)
        {
            foreach (IStaminaDamageable staminaDamageable in Combat.DetectedStaminaDamageables.ToList())
            {
                staminaDamageable.TakeStaminaDamage(damageStaminaAmount, core.transform.position, blockable);
            }
        }
    }
    protected void Jump()
    {
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
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.jumpInpusStopYSpeedMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

}
