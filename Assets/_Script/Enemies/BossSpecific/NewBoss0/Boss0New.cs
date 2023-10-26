using UnityEngine;

public class Boss0New : BossBase
{
    public B0N_Idle IdleState { get; private set; }
    public B0N_AngryState AngryState { get; private set; }
    public B0N_InitAnim InitAnim { get; private set; }

    public B0N_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }

    public B0N_NormalAttackState1 NormalAttackState1 { get; private set; }
    public B0N_NormalAttackState2 NormalAttackState2 { get; private set; }
    public B0N_StrongAttackState StrongAttackState { get; private set; }

    public B0N_ChargeState ChargeState { get; private set; }
    public B0N_MultiAttackState MultiAttackState { get; private set; }

    public B0N_StunState StunState { get; private set; }
    public B0N_KinematicState KinematicState { get; private set; }
    public B0N_DeadState DeadState { get; private set; }

    [SerializeField] private B0N_StateData stateData;
    [SerializeField] private Transform meleeAttackPosition;
    

    public override void Awake()
    {
        base.Awake();

        IdleState = new B0N_Idle(this, StateMachine, "idle", stateData.idleStateData, this);
        AngryState = new B0N_AngryState(this, StateMachine, "angry", this);
        InitAnim = new B0N_InitAnim(this, StateMachine, "init", this);

        PlayerDetectedMoveState = new B0N_PlayerDetectedMoveState(this, StateMachine, "move", stateData.detectedMoveStateData, this);
        NormalAttackState1 = new B0N_NormalAttackState1(this, StateMachine, "normalAttack1", meleeAttackPosition, stateData.normalAttack1StateData, this);
        NormalAttackState2 = new B0N_NormalAttackState2(this, StateMachine, "normalAttack2", meleeAttackPosition, stateData.normalAttack2StateData, this);
        StrongAttackState = new B0N_StrongAttackState(this, StateMachine, "strongAttack", meleeAttackPosition, stateData.strongAttackStateData, this);

        ChargeState = new B0N_ChargeState(this, StateMachine, "charge", stateData.ChargeState, this);
        MultiAttackState = new B0N_MultiAttackState(this, StateMachine, "multiAttack", meleeAttackPosition, stateData.MultiAttackState, this);


        KinematicState = new B0N_KinematicState(this, StateMachine, "kinematic", this);
        DeadState = new B0N_DeadState(this, StateMachine, "dead", this);
    }
}
