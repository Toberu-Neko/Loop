using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_AngryMagicState : EnemyWaitForAnimFinishState
{
    private Boss0New boss;
    private ED_TimeSlowSkill stateData;
    private float coolDown;

    public B0N_AngryMagicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0New boss, ED_TimeSlowSkill angrySkillData) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
        stateData = angrySkillData;
    }

    public override void Enter()
    {
        base.Enter();

        coolDown = Random.Range(stateData.minCooldown, stateData.maxCooldown);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        boss.EnterSlowTriggerOn(Time.time);
        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }

    public bool CheckCanAttack()
    {
        return EndTime == 0f || Time.time >= EndTime + coolDown;
    }
}
