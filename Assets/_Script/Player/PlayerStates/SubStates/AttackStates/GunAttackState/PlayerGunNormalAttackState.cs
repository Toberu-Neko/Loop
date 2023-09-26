using UnityEngine;

public class PlayerGunNormalAttackState : PlayerGunAttackState
{
    private SO_WeaponData_Gun data;

    private int xInput;
    private Vector2 mouseDirectionInput;

    public PlayerGunNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;
        xInput = player.InputHandler.NormInputX;
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
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        if(player.PlayerWeaponManager.GunCurrentEnergy >= data.energyCostPerShot)
        {
            player.PlayerWeaponManager.DecreaseEnergy();
            player.PlayerWeaponManager.GunFiredRegenDelay();

            PlayerProjectile proj = ObjectPoolManager.SpawnObject(data.bulletObject, player.PlayerWeaponManager.ProjectileStartPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles).GetComponent<PlayerProjectile>();
            ProjectileDetails details = data.normalAttackDetails;
            details.damageAmount *= PlayerInventoryManager.Instance.GunMultiplier.damageMultiplier;
            proj.Fire(details, mouseDirectionInput);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    public bool CheckCanAttack()
    {
        return (StartTime == 0f || Time.time >= StartTime + data.attackSpeed) && player.PlayerWeaponManager.GunCurrentEnergy >= data.energyCostPerShot;
    }
}
