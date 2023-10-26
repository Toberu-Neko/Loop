using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E1_StateData", menuName = "Data/Entity Data/Enemies/E1 State Data")]
public class E1_StateData : BaseEnemyStateData
{
    [Header("Movement")]
    public ED_EnemyIdleState idleStateData;
    public ED_EnemyGroundMoveState groundMoveStateData;
    public ED_PlayerDetectedState playerDetectedStateData;
    
    [Header("Attack")]
    public ED_EnemyChargeState chargeStateData;
    public ED_EnemyLookForPlayerState lookForPlayerStateData;
    public ED_EnemyMeleeAttackState meleeAttackStateData;

    [Header("Combat")]
    public ED_EnemyStunState stunStateData;
    public ED_EnemyDeadState deadStateData;

}
