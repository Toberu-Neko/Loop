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
    private event Action OnAttackMapItems;

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
        OnAttack += player.TimeSkillManager.HandleOnAttack;
        OnAttack += CamManager.Instance.CameraShake;

        OnAttackMapItems += CamManager.Instance.CameraShake;
    }

    public override void Exit()
    {
        base.Exit();
        Stats.SetCanChangeWeapon(true);
        OnAttack -= player.TimeSkillManager.HandleOnAttack;
        OnAttack -= CamManager.Instance.CameraShake;

        OnAttackMapItems -= CamManager.Instance.CameraShake;


        Combat.DetectedDamageables.Clear();
        Combat.DetectedKnockbackables.Clear();
        Combat.DetectedStaminaDamageables.Clear();
        Combat.DetectedMapDamageableItems.Clear();
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

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger(); 
        

    }

    public void DoDamageToDamageList(WeaponType weaponType, float damageAmount,float damageStaminaAmount ,Vector2 knockBackAngle, float knockBackForce, bool blockable = true)
    {
        if(weaponType == WeaponType.Fist)
        {
            damageAmount *= PlayerInventoryManager.Instance.FistMultiplier.damageMultiplier;
        }
        else if(weaponType == WeaponType.Sword)
        {
            damageAmount *= PlayerInventoryManager.Instance.SwordMultiplier.damageMultiplier;
        }
        else if(weaponType == WeaponType.Gun)
        {
            damageAmount *= PlayerInventoryManager.Instance.GunMultiplier.damageMultiplier;
        }

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
                knockbackable.Knockback(knockBackAngle, knockBackForce, Movement.ParentTransform.position, blockable);
            }
        }

        if(Combat.DetectedStaminaDamageables.Count > 0)
        {
            foreach (IStaminaDamageable staminaDamageable in Combat.DetectedStaminaDamageables.ToList())
            {
                staminaDamageable.TakeStaminaDamage(damageStaminaAmount, core.transform.position, blockable);
            }
        }

        if(Combat.DetectedMapDamageableItems.Count > 0)
        {
            OnAttackMapItems?.Invoke();
            foreach (IMapDamageableItem mapDamageableItem in Combat.DetectedMapDamageableItems.ToList())
            {
                mapDamageableItem.TakeDamage(damageAmount);
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

public enum WeaponType
{
    Sword,
    Gun,
    Fist
}
