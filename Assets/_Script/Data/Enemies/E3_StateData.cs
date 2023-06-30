using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E3_StateData", menuName = "Data/Entity Data/E3 Human Blockable Data")]
public class E3_StateData : D_BaseEnemyStateData
{
    [Header("Movement")]
    public S_EnemyIdleState idleStateData;
    public S_EnemyGroundMoveState groundMoveStateData;
    public S_EnemyPlayerDetectedState playerDetectedStateData;
    public S_EnemyShieldMoveState shieldMoveStateData;

    [Header("Attack")]
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
