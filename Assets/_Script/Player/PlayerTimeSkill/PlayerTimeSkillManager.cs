using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkillManager : MonoBehaviour
{
    [SerializeField] private PlayerTimeSkillData data;
    private Player player;

    public PlayerTimeSkillStateMachine StateMachine { get; private set; }
    public PlayerTimeSkill_None SkillNone { get; private set; }
    public PlayerTimeSkill_RewindPlayer SkillRewindPlayer { get; private set; }
    public PlayerTimeSkill_TimeStopAll SkillTimeStopAll { get; private set; }  

    public PlayerTimeSkill_BookMark SkillBookMark { get; private set; }

    public event Action OnStateChanged;

    private float maxEnergy;
    public float CurrentEnergy { get; private set; }


    private void Awake()
    {
        player = GetComponent<Player>();

        maxEnergy = data.maxEnergy;
        CurrentEnergy = maxEnergy;
    }
    private void Start()
    {
        StateMachine = new();
        SkillNone = new(player, this, StateMachine, data, "None");
        SkillRewindPlayer = new(player, this, StateMachine, data, "RewindPlayer");
        SkillBookMark = new(player, this, StateMachine, data, "BookMark");
        SkillTimeStopAll = new(player, this, StateMachine, data, "TimeStopAll");

        StateMachine.Initialize(SkillNone);
        OnStateChanged?.Invoke();
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void DecreaseEnergy(float amount)
    {
        CurrentEnergy -= amount;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, maxEnergy);
        OnStateChanged?.Invoke();
    }

    public void ChangeToNoneSkill()
    {
        StateMachine.ChangeState(SkillNone);
        OnStateChanged?.Invoke();
    }

    public void ChangeToRewindSkill()
    {
        StateMachine.ChangeState(SkillRewindPlayer);
        OnStateChanged?.Invoke();
    }

    public void ChangeToBookMarkSkill()
    {
        StateMachine.ChangeState(SkillBookMark);
        OnStateChanged?.Invoke();
    }

    public void ChangeToTimeStopSkill()
    {
        StateMachine.ChangeState(SkillTimeStopAll);
        OnStateChanged?.Invoke();
    }
}


