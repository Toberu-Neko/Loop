using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkillManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private PlayerTimeSkillData data;
    [SerializeField] private Core core;

    public PlayerTimeSkills UnlockedTimeSkills { get; private set; }

    private Combat combat;

    [field: SerializeField] public GameObject BulletTimeEffectObj { get; private set; }
    private Player player;

    public PlayerTimeSkillStateMachine StateMachine { get; private set; }
    public PlayerTimeSkill_None SkillNone { get; private set; }
    public PlayerTimeSkill_RewindPlayer SkillRewindPlayer { get; private set; }
    public PlayerTimeSkill_TimeStopAll SkillTimeStopAll { get; private set; }  
    public PlayerTimeSkill_TimeStopThrow SkillTimeStopThrow { get; private set; }
    public PlayerTimeSkill_BookMark SkillBookMark { get; private set; }
    public PlayerTimeSkill_BulletTimeAll SkillBulletTimeAll { get; private set; }
    public PlayerTimeSkill_BulletTimeRanged SkillBulletTimeRanged { get; private set; }

    public GameObject[] PredictLineObjects { get; private set; }
    public Transform[] PredictLineTransforms { get; set; }
    [SerializeField] private Transform predictObjMother;

    private float maxEnergy;
    public float CurrentEnergy { get; private set; }

    public event Action OnStateChanged;

    private void Awake()
    {
        player = GetComponent<Player>();
        combat = core.GetCoreComponent<Combat>();


        BulletTimeEffectObj.SetActive(false);

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

        StateMachine = new();
        SkillNone = new(player, this, StateMachine, data, "None");
        SkillRewindPlayer = new(player, this, StateMachine, data, "RewindPlayer");
        SkillBookMark = new(player, this, StateMachine, data, "BookMark");
        SkillTimeStopAll = new(player, this, StateMachine, data, "TimeStopAll");
        SkillTimeStopThrow = new(player, this, StateMachine, data, "TimeStopThrow");
        SkillBulletTimeAll = new(player, this, StateMachine, data, "BulletTimeAll");
        SkillBulletTimeRanged = new(player, this, StateMachine, data, "BulletTimeRanged");

        StateMachine.Initialize(SkillNone);
        OnStateChanged?.Invoke();
    }

    private void Start()
    {
        PlayerInventoryManager.Instance.OnTimeSkillChanged += UpdateUnlockedSkills;
        UpdateUnlockedSkills();
    }

    private void UpdateUnlockedSkills()
    {
        UnlockedTimeSkills = new();

        foreach (var item in PlayerInventoryManager.Instance.TimeSkillItemInventory)
        {
            if (ItemDataManager.Instance.TimeSkillDict.TryGetValue(item.Key, out SO_TimeSkillItem timeSkillItem))
            {
                var unlockedSkills = timeSkillItem.unlockedSkills;

                UnlockedTimeSkills.timeSlowRanged |= unlockedSkills.timeSlowRanged;
                UnlockedTimeSkills.timeSlowAll |= unlockedSkills.timeSlowAll;
                UnlockedTimeSkills.timeStopRanged |= unlockedSkills.timeStopRanged;
                UnlockedTimeSkills.timeStopAll |= unlockedSkills.timeStopAll;
                UnlockedTimeSkills.timeReverse |= unlockedSkills.timeReverse;
                UnlockedTimeSkills.bookMark |= unlockedSkills.bookMark;
            }
            else
            {
                Debug.LogError("The time skill item name in PlayerInventoryManager.Instance.TimeSkillItemInventory is wrong");
            }
        }

        int unlockedSkillsCount = 0;
        if (UnlockedTimeSkills.timeSlowRanged) unlockedSkillsCount++;
        if (UnlockedTimeSkills.timeSlowAll) unlockedSkillsCount++;
        if (UnlockedTimeSkills.timeStopRanged) unlockedSkillsCount++;
        if (UnlockedTimeSkills.timeStopAll) unlockedSkillsCount++;
        if (UnlockedTimeSkills.timeReverse) unlockedSkillsCount++;
        if (UnlockedTimeSkills.bookMark) unlockedSkillsCount++;

        if(unlockedSkillsCount == 1)
        {
            if (UnlockedTimeSkills.timeSlowRanged) ChangeToBulletTimeRangedSkill();
            if (UnlockedTimeSkills.timeSlowAll) ChangeToBulletTimeAllSkill();
            if (UnlockedTimeSkills.timeStopRanged) ChangeToTimeStopThrowSkill();
            if (UnlockedTimeSkills.timeStopAll) ChangeToTimeStopSkill();
            if (UnlockedTimeSkills.timeReverse) ChangeToRewindSkill();
            if (UnlockedTimeSkills.bookMark) ChangeToBookMarkSkill();
        }
    }

    public void SetTimeEnergyMax()
    {
        CurrentEnergy = maxEnergy;
    }
    

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnEnable()
    {
        combat.OnPerfectBlock += HandleOnPerfectBlock;
    }

    private void OnDisable()
    {
        combat.OnPerfectBlock -= HandleOnPerfectBlock;
        PlayerInventoryManager.Instance.OnTimeSkillChanged -= UpdateUnlockedSkills;
    }

    public void HandleOnAttack()
    {
        IncreaseEnergy(data.attackInvreaseEnergy);
    }

    private void HandleOnPerfectBlock()
    {
        IncreaseEnergy(data.perfectBolckIncreaseEnergy);
    }

    public void DecreaseEnergy(float amount)
    {
        CurrentEnergy -= amount;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, maxEnergy);
        OnStateChanged?.Invoke();
    }

    public void IncreaseEnergy(float amount)
    {
        CurrentEnergy += amount;
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

    public void ChangeToBulletTimeRangedSkill()
    {
        StateMachine.ChangeState(SkillBulletTimeRanged);
        OnStateChanged?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.bulletTimeRangedRadius);
    }

    public void LoadData(GameData data)
    {
        switch (data.currentTimeSkill)
        {
            case "PlayerTimeSkill_None":
                ChangeToNoneSkill();
                break;
            case "PlayerTimeSkill_RewindPlayer":
                ChangeToRewindSkill();
                break;
            case "PlayerTimeSkill_BookMark":
                ChangeToBookMarkSkill();
                break;
            case "PlayerTimeSkill_TimeStopAll":
                ChangeToTimeStopSkill();
                break;
            case "PlayerTimeSkill_TimeStopThrow":
                ChangeToTimeStopThrowSkill();
                break;
            case "PlayerTimeSkill_BulletTimeAll":
                ChangeToBulletTimeAllSkill();
                break;
            case "PlayerTimeSkill_BulletTimeRanged":
                ChangeToBulletTimeRangedSkill();
                break;
            default:
                Debug.LogError("The current time skill name in gamedata is worng");
                break;
        }
    }

    public void SaveData(GameData data)
    {
        data.currentTimeSkill = StateMachine.CurrentState.ToString();
    }
}


