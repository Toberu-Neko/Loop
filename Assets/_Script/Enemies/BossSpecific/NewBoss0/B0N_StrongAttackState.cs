using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_StrongAttackState : UnblockableAttackState
{
    private Boss0New boss;
    public B0N_StrongAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }


    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        AudioManager.Instance.PlaySoundFX(boss.StateData.strongAttackSFX, Movement.ParentTransform, AudioManager.SoundType.threeD);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }

    public override void AnimationDangerParticleTrigger()
    {
        base.AnimationDangerParticleTrigger();

        boss.SpawnDnagerParticle();
    }
}
