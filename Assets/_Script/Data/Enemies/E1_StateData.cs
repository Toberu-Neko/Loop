using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E1_StateData", menuName = "Data/Entity Data/E1 State Data")]
public class E1_StateData : D_BaseEnemyStateData
{
    [Header("Movement")]
    public S_EnemyIdleState idleStateData;
    public S_EnemyGroundMoveState groundMoveStateData;
    public S_EnemyPlayerDetectedState playerDetectedStateData;
    
    [Header("Attack")]
    public S_EnemyChargeState chargeStateData;
    public S_EnemyLookForPlayerState lookForPlayerStateData;
    public S_EnemyMeleeAttackState meleeAttackStateData;

    [Header("Combat")]
    public S_EnemyStunState stunStateData;
    public S_EnemyDeadState deadStateData;

    private void OnEnable()
    {
        meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
