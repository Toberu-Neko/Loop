using UnityEngine;

public class PlayerFistCounterAttackState : PlayerFistAttackState
{
    private SO_WeaponData_Fist data;
    public PlayerFistCounterAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetPerfectBlockAttackFalse();
        Time.timeScale = 0.75f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public override void Exit()
    {
        base.Exit();

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement)
        {
            Movement.SetVelocityX(data.counterAttackDetails.movementSpeed * Movement.FacingDirection);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Fist, data.counterAttackDetails);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }
}
