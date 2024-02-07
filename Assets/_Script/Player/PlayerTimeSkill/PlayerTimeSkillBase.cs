using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class PlayerTimeSkillBase
{
    protected Core core;
    protected Combat Combat;
    protected Stats Stats;
    protected Movement Movement;

    protected Player player;
    protected PlayerTimeSkillStateMachine stateMachine;
    protected PlayerTimeSkillManager manager;
    public LocalizedString SkillName { get; protected set; }
    public PlayerTimeSkillData Data { get; protected set; }

    private string animBoolName;

    public PlayerTimeSkillBase(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.Data = data;
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
