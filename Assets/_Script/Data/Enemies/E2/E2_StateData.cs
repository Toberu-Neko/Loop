using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E2_StateData", menuName = "Data/Entity Data/Enemies/E2 State Data")]
public class E2_StateData : BaseEnemyStateData
{
    [Header("Movement")]
    public S_EnemyIdleState idleStateData;
    public S_EnemyGroundMoveState groundMoveStateData;
    public S_EnemyPlayerDetectedState playerDetectedStateData;
    public S_EnemyLookForPlayerState lookForPlayerStateData;
    
    [Header("Attack")]
    public S_EnemyRangedAttackState rangedAttackStateData;
    public S_EnemyMeleeAttackState meleeAttackStateData;

    [Header("Combat")]
    public S_EnemyStunState stunStateData;
    public S_EnemyDeadState deadStateData;
    public S_EnemyDodgeState dodgeStateData;

    private void OnEnable()
    {
        meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
        rangedAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
