using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_MultiAttackState : MultiAttackState
{
    private Boss0New boss;
    public B0N_MultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_MultiAttackState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        AudioManager.Instance.PlaySoundFX(boss.StateData.multiSkillSFX, Movement.ParentTransform, AudioManager.SoundType.threeD);
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
