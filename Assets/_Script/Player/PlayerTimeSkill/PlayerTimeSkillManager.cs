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
    public PlayerTimeSkill_TimeStopThrow SkillTimeStopThrow { get; private set; }
    public PlayerTimeSkill_BookMark SkillBookMark { get; private set; }
    public PlayerTimeSkill_BulletTimeAll SkillBulletTimeAll { get; private set; }

    public GameObject[] PredictLineObjects { get; private set; }
    public Transform[] PredictLineTransforms { get; set; }
    [SerializeField] private Transform predictObjMother;

    private float maxEnergy;
    public float CurrentEnergy { get; private set; }

    public event Action OnStateChanged;

    private void Awake()
    {
        player = GetComponent<Player>();

        maxEnergy = data.maxEnergy;
        CurrentEnergy = maxEnergy;

        PredictLineObjects = new GameObject[data.numberOfPredictLineObj];
        PredictLineTransforms = new Transform[data.numberOfPredictLineObj];
        for (int i = 0; i < data.numberOfPredictLineObj; i++)
        {
            PredictLineObjects[i] = Instantiate(data.predictLineObj, transform.position, Quaternion.identity, predictObjMother);
            PredictLineTransforms[i] = PredictLineObjects[i].transform;
            PredictLineObjects[i].SetActive(false);
        }
    }
    private void Start()
    {
        StateMachine = new();
        SkillNone = new(player, this, StateMachine, data, "None");
        SkillRewindPlayer = new(player, this, StateMachine, data, "RewindPlayer");
        SkillBookMark = new(player, this, StateMachine, data, "BookMark");
        SkillTimeStopAll = new(player, this, StateMachine, data, "TimeStopAll");
        SkillTimeStopThrow = new(player, this, StateMachine, data, "TimeStopThrow");
        SkillBulletTimeAll = new(player, this, StateMachine, data, "BulletTimeAll");

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

    public void ChangeToTimeStopThrowSkill()
    {
        StateMachine.ChangeState(SkillTimeStopThrow);
        OnStateChanged?.Invoke();
    }

    public void ChangeToBulletTimeAllSkill()
    {
        StateMachine.ChangeState(SkillBulletTimeAll);
        OnStateChanged?.Invoke();
    }
}


