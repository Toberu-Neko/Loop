using UnityEngine;

public class PlayerThrowGrenadeState : PlayerGunAttackState
{
    SO_WeaponData_Gun data;
    private int xInput;

    private bool canGoNextState;
    public PlayerThrowGrenadeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        player.WeaponManager.DecreaseEnergy();
        canGoNextState = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        Movement.SetVelocityX(playerData.movementVelocity * xInput);
        Movement.CheckIfShouldFlip(xInput);

        Jump();

        if((xInput != 0 || player.InputHandler.JumpInput) && canGoNextState)
        {
            isAttackDone = true;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        ThrowGrenade();
        canGoNextState = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    private void ThrowGrenade()
    {
        var obj = ObjectPoolManager.SpawnObject(data.grenadeObj, player.WeaponManager.ProjectileStartPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
        PlayerGrenade script = obj.GetComponent<PlayerGrenade>();
        script.Throw(data.grenadeDetails, Movement.FacingDirection);
    }

}
