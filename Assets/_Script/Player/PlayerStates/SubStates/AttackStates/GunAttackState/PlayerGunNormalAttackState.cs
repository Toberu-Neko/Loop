using UnityEngine;

public class PlayerGunNormalAttackState : PlayerGunAttackState
{
    private SO_WeaponData_Gun data;

    private int xInput;
    private Vector2 mouseDirectionInput;
    private bool shot;
    public PlayerGunNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;
        xInput = player.InputHandler.NormInputX;
        player.Anim.SetFloat("mouseDegree", mouseDirectionInput.y);
        shot = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        mouseDirectionInput = player.InputHandler.RawMouseDirectionInput;
        xInput = player.InputHandler.NormInputX;
        player.Anim.SetFloat("mouseDegree", mouseDirectionInput.y);
        if(Movement.CurrentVelocity.x < 0)
        {
            player.Anim.SetFloat("xVelocity", -Movement.CurrentVelocity.x);
        }
        else
        {
            player.Anim.SetFloat("xVelocity", Movement.CurrentVelocity.x);
        }

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

        Shoot();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    private void Shoot()
    {
        if (player.WeaponManager.GunCurrentNormalAttackEnergy >= data.energyCostPerShot && !shot)
        {
            shot = true;
            AudioManager.instance.PlaySoundFX(player.PlayerSFX.gunAttack, player.transform);
            player.WeaponManager.DecreaseGunNormalAttackEnergy();
            player.WeaponManager.GunFiredRegenDelay();

            PlayerProjectile proj = ObjectPoolManager.SpawnObject(data.bulletObject, player.WeaponManager.ProjectileStartPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles).GetComponent<PlayerProjectile>();
            ProjectileDetails details = new();
            details.duration = data.normalAttackDetails.duration;
            details.speed = data.normalAttackDetails.speed;
            details.combatDetails = new();
            details.combatDetails.damageAmount = data.normalAttackDetails.combatDetails.damageAmount * PlayerInventoryManager.Instance.GunMultiplier.damageMultiplier;
            details.combatDetails.staminaDamageAmount = data.normalAttackDetails.combatDetails.staminaDamageAmount;
            details.combatDetails.knockbackStrength = data.normalAttackDetails.combatDetails.knockbackStrength;
            details.combatDetails.knockbackAngle = data.normalAttackDetails.combatDetails.knockbackAngle;
            details.combatDetails.blockable = data.normalAttackDetails.combatDetails.blockable;


            details.combatDetails.damageAmount *= PlayerInventoryManager.Instance.GunMultiplier.damageMultiplier;

            if(mouseDirectionInput == Vector2.zero)
            {
                if (Movement.FacingDirection == 1)
                    mouseDirectionInput = Vector2.right;
                else
                    mouseDirectionInput = Vector2.left;
            }
            proj.Fire(details, mouseDirectionInput);
        }

    }

    public bool CheckCanAttack()
    {
        return (StartTime == 0f || Time.time >= StartTime + data.attackSpeed) && player.WeaponManager.GunCurrentNormalAttackEnergy >= data.energyCostPerShot;
    }
}
