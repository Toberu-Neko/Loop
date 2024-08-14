using UnityEngine;
using UnityEngine.Localization;

public class Boss0New : BossBase
{
    #region States
    public B0N_Idle IdleState { get; private set; }
    public B0N_AngryState AngryState { get; private set; }
    public B0N_InitAnim InitAnim { get; private set; }

    public B0N_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }

    public B0N_AngryMagicState AngryMagicState { get; private set; }
    public B0N_NormalAttackState1 NormalAttackState1 { get; private set; }
    public B0N_NormalAttackState2 NormalAttackState2 { get; private set; }
    public B0N_StrongAttackState StrongAttackState { get; private set; }

    public B0N_PreChargeState PreChargeState { get; private set; }
    public B0N_ChargeState ChargeState { get; private set; }
    public B0N_AfterChargeState AfterChargeState { get; private set; }
    public B0N_MultiAttackState MultiAttackState { get; private set; }

    public B0N_StunState StunState { get; private set; }
    public B0N_KinematicState KinematicState { get; private set; }
    public B0N_DeadState DeadState { get; private set; }
    #endregion

    [field: SerializeField] public Transform DeadParticleTrans { get; private set; }
    [field: SerializeField] public B0N_StateData StateData { get; private set; }
    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform dangerAttackParticlePos;
    [SerializeField] private GameObject dangerParticlePrefab;

    [field: SerializeField] public GameObject EnterSlowTrigger { get; private set; }
    private float slowOnTimer;
    // Exit door will be activated when the boss is defeated
    [SerializeField] private GameObject exitDoor;

    private bool saidTimeSkillTu;
    [Header("Things to Say")]
    [SerializeField] private LocalizedString firstActivateTimeSkillText;
    [SerializeField] private LocalizedString firstCloseTimeSkillText;
    

    public override void Awake()
    {
        base.Awake();

        if(exitDoor != null)
            exitDoor.SetActive(false);

        saidTimeSkillTu = false;

        #region New States
        IdleState = new B0N_Idle(this, StateMachine, "idle", StateData.idleStateData, this);
        AngryState = new B0N_AngryState(this, StateMachine, "angry", this);
        InitAnim = new B0N_InitAnim(this, StateMachine, "init", this);

        PlayerDetectedMoveState = new B0N_PlayerDetectedMoveState(this, StateMachine, "move", StateData.detectedMoveStateData, this);
        NormalAttackState1 = new B0N_NormalAttackState1(this, StateMachine, "normalAttack1", meleeAttackPosition, StateData.normalAttack1StateData, this);
        NormalAttackState2 = new B0N_NormalAttackState2(this, StateMachine, "normalAttack2", meleeAttackPosition, StateData.normalAttack2StateData, this);
        StrongAttackState = new B0N_StrongAttackState(this, StateMachine, "strongAttack", meleeAttackPosition, StateData.strongAttackStateData, this);
        AngryMagicState = new B0N_AngryMagicState(this, StateMachine, "angryMagic", this, StateData.angrySkillData);

        PreChargeState = new B0N_PreChargeState(this, StateMachine, "preCharge", this);
        ChargeState = new B0N_ChargeState(this, StateMachine, "charge", StateData.ChargeState, this);
        AfterChargeState = new B0N_AfterChargeState(this, StateMachine, "afterCharge", this);
        MultiAttackState = new B0N_MultiAttackState(this, StateMachine, "multiAttack", meleeAttackPosition, StateData.MultiAttackState, this);

        StunState = new B0N_StunState(this, StateMachine, "stun", StateData.stunStateData, this);
        KinematicState = new B0N_KinematicState(this, StateMachine, "stun", this);
        DeadState = new B0N_DeadState(this, StateMachine, "dead", this);
        #endregion

        slowOnTimer = 0f;
        EnterSlowTrigger.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    public override void Update()
    {
        base.Update();

        Stats.Timer(slowOnTimer);

        if (EnterSlowTrigger.activeInHierarchy && Time.time >= slowOnTimer + StateData.angrySkillData.duration)
        {
            if (!saidTimeSkillTu)
            {
                if(!firstCloseTimeSkillText.IsEmpty)
                    UI_Manager.Instance.ActivateTutorialPopUpUI(firstCloseTimeSkillText);
                saidTimeSkillTu = true;
            }
            EnterSlowTrigger.SetActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Stats.Stamina.OnCurrentValueZero += HandlePoiseZero;
        Stats.Health.OnCurrentValueZero += HandleHealthZero;
        OnEnterBossRoom += HandleEnterBossRoom;
        OnAlreadyDefeated += HandleAlreadyDefeated;

        Combat.OnGoToKinematicState += GotoKinematicState;
        Combat.OnGoToStunState += OnGotoStunState;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        Stats.Stamina.OnCurrentValueZero -= HandlePoiseZero;
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
        OnEnterBossRoom -= HandleEnterBossRoom;
        OnAlreadyDefeated -= HandleAlreadyDefeated;

        Combat.OnGoToKinematicState -= GotoKinematicState;
        Combat.OnGoToStunState -= OnGotoStunState;
    }

    public void EnterSlowTriggerOn(float time)
    {
        if (!saidTimeSkillTu)
        {
            if(!firstActivateTimeSkillText.IsEmpty)
                UI_Manager.Instance.ActivateTutorialPopUpUI(firstActivateTimeSkillText);
        }
        EnterSlowTrigger.SetActive(true);
        slowOnTimer = time;
    }


    private void GotoKinematicState(float time)
    {
        KinematicState.SetTimer(time);
        StateMachine.ChangeState(KinematicState);
    }

    private void OnGotoStunState()
    {
        if (Stats.Health.CurrentValue > 0)
            StateMachine.ChangeState(StunState);
        else
            StateMachine.ChangeState(DeadState);
    }

    private void HandlePoiseZero()
    {
        if (Stats.Health.CurrentValue <= 0 || 
            StateMachine.CurrentState == KinematicState || 
            StateMachine.CurrentState == AngryState)
        {
            Debug.Log("Cant be stunned");
            return;
        }

        if (Stats.Health.CurrentValue <= 0)
        {
            StateMachine.ChangeState(DeadState);
        }
        else
        {
            StateMachine.ChangeState(StunState);
        }
    }

    private void HandleHealthZero()
    {
        if (StateMachine.CurrentState == KinematicState)
            return;
        StateMachine.ChangeState(DeadState);
    }

    private new void HandleEnterBossRoom()
    {
        StateMachine.ChangeState(InitAnim);
    }

    public void HandleAlreadyDefeated()
    {
        if (exitDoor != null)
            exitDoor.SetActive(true);
    }

    public void SpawnDnagerParticle()
    {
        ObjectPoolManager.SpawnObject(dangerParticlePrefab, dangerAttackParticlePos.position, Quaternion.identity);
    }
}
