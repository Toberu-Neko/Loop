using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunCounterState : PlayerGunAttackState
{
    private SO_WeaponData_Gun data;
    private bool shot;
    private Vector2 mouseDirectionInput;
    public PlayerGunCounterState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();
        shot = false;
        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;

        Stats.SetPerfectBlockAttackFalse();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded)
        {
            Movement.SetVelocityZero();
        }

        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;

        if (player.InputHandler.NormInputX != 0 && shot)
        {
            isAttackDone = true;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        shot = true;

        player.PlayerWeaponManager.GunFiredRegenDelay();

        PlayerProjectile proj = ObjectPoolManager.SpawnObject(data.bulletObject, player.PlayerWeaponManager.ProjectileStartPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles).GetComponent<PlayerProjectile>();
        ProjectileDetails details = data.counterAttackDetails;
        details.damageAmount *= PlayerInventoryManager.Instance.GunMultiplier.damageMultiplier;
        proj.Fire(details, mouseDirectionInput);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }
}
