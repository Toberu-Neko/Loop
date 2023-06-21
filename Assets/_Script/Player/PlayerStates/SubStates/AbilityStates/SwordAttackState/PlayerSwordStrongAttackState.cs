using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordStrongAttackState : PlayerAbilityState
{
    private SO_WeaponData_Sword weaponData;
    public PlayerSwordStrongAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        weaponData = player.PlayerWeaponManager.SwordData;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        GameObject projectile = GameObject.Instantiate(player.PlayerWeaponManager.SwordData.projectile, core.transform.position, core.transform.parent.rotation);
        SwordProjectile projectileScript = projectile.GetComponent<SwordProjectile>();
        weaponData.projectileDetails.facingDirection = Movement.FacingDirection;
        projectileScript.Fire(weaponData.projectileDetails);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();
    }
}
