using UnityEngine;

public class PlayerPerfectBlockState : PlayerAttackState
{
    public PlayerPerfectBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        Collider2D[] enemy = Physics2D.OverlapCircleAll(player.transform.position, playerData.perfectBlockKnockbackRadius, playerData.whatIsEnemy);

        foreach (Collider2D enemyCollider in enemy)
        {
            IKnockbackable knockbackable = enemyCollider.GetComponentInChildren<IKnockbackable>();
            knockbackable?.Knockback(playerData.perfectBlockKnockbackAngle ,playerData.perfectBlockKnockbackForce, Movement.ParentTransform.position);    
        }

        Collider2D[] projectiles = Physics2D.OverlapCircleAll(player.transform.position, playerData.perfectBlockKnockbackRadius, playerData.whatIsEnemyProjectile);

        foreach(var item in projectiles)
        {
            item.TryGetComponent(out IFireable fireable);
            fireable?.HandlePerfectBlock();
        }
    }
}
