using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_DeadState : DeadState
{
    private Boss0New boss0New;
    private bool isDead;
    private float animFinishTime;
    public B0N_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0New boss0New) : base(entity, stateMachine, animBoolName)
    {
        this.boss0New = boss0New;
        isDead = false;
    }

    public override void Enter()
    {
        base.Enter();

        animFinishTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        boss0New.HandleAlreadyDefeated();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= animFinishTime + 0.5f && !isDead && animFinishTime != 0)
        {
            isDead = true;
            Death.Die();
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        animFinishTime = Time.time;
        boss0New.spriteRenderer.enabled = false;
        ParticleManager.StartParticlesWithRandomRotation(boss0New.DeathParticles, boss0New.DeadParticleTrans.position);
    }
}
