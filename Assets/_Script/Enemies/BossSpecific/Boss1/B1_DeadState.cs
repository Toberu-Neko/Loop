using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_DeadState : DeadState
{
    private Boss1 boss;
    private bool isDead;
    private float animFinishTime;
    public B1_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        isDead = false;
        ParticleManager.StartParticlesWithRandomRotation(boss.DeathParticles);
        boss.spriteRenderer.enabled = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= StartTime + 0.5f && !isDead)
        {
            isDead = true;
            Death.Die();
        }
    }
}
