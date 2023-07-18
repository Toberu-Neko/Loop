using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkillBase
{
    protected Core core;
    protected Combat Combat;
    protected Stats Stats;
    protected Movement Movement;

    protected Player player;
    protected PlayerTimeSkillStateMachine stateMachine;
    protected PlayerTimeSkillData data;
    protected PlayerTimeSkillManager manager;
    private string animBoolName;

    public PlayerTimeSkillBase(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.data = data;
        this.animBoolName = animBoolName;
        this.manager = manager;

        core = player.Core;
        Combat = player.Core.GetCoreComponent<Combat>();
        Movement = player.Core.GetCoreComponent<Movement>();
        Stats = player.Core.GetCoreComponent<Stats>();
    }

    public virtual void Enter()
    {
    }
    public virtual void Exit()
    {
    }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
