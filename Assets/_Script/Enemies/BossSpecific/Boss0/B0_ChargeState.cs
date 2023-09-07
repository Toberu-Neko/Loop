using UnityEngine;

public class B0_ChargeState : ChargeState
{
    private Boss0 boss;
    public B0_ChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyChargeState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
}
