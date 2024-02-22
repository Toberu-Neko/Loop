using UnityEngine;

public class Enemy7 : Entity
{
    public E7_IdleState IdleState { get; private set; }
    public E7_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E7_SnipingState SnipingState { get; private set; }
    public E7_DeadState DeadState { get; private set; }

    [SerializeField] private E7_StateData stateData;
    [SerializeField] private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        IdleState = new E7_IdleState(this, StateMachine, "idle", stateData.idleStateData, this);
        PlayerDetectedState = new E7_PlayerDetectedState(this, StateMachine, "move", stateData.playerDetectedState, this);
        SnipingState = new E7_SnipingState(this, StateMachine, "sniping", rangedAttackPosition, stateData.snipingStateData, this);
        DeadState = new E7_DeadState(this, StateMachine, "dead");
    }

    protected override void Start()
    {
        base.Start();

        movement.SetRBKinematic();
        StateMachine.Initialize(IdleState);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        Combat.SetPerfectBlockAllDir(true);
        Stats.Health.OnCurrentValueZero += HandleHealthZero;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
    }

    private void HandleHealthZero()
    {
        StateMachine.ChangeState(DeadState);
    }
}
