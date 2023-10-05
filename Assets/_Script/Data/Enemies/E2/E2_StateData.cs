using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E2_StateData", menuName = "Data/Entity Data/Enemies/E2 State Data")]
public class E2_StateData : BaseEnemyStateData
{
    [Header("Movement")]
    public ED_EnemyIdleState idleStateData;
    public ED_EnemyGroundMoveState groundMoveStateData;
    public ED_EnemyPlayerDetectedState playerDetectedStateData;
    public ED_EnemyLookForPlayerState lookForPlayerStateData;
    
    [Header("Attack")]
    public ED_EnemyRangedAttackState rangedAttackStateData;
    public ED_EnemyMeleeAttackState meleeAttackStateData;

    [Header("Combat")]
    public ED_EnemyStunState stunStateData;
    public ED_EnemyDeadState deadStateData;
    public ED_EnemyDodgeState dodgeStateData;

    private void OnEnable()
    {
        meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
