using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E4_StateData", menuName = "Data/Entity Data/Enemies/E4 Human Melee Normal")]
public class E4_StateData : BaseEnemyStateData
{
    [Header("Movement")]
    public S_EnemyIdleState idleStateData;
    public S_EnemyGroundMoveState groundMoveStateData;
    public S_EnemyPlayerDetectedState playerDetectedStateData;

    [Header("Attack")]
    public S_EnemyMeleeAttackState meleeAttackStateData;
    public S_EnemyLookForPlayerState lookForPlayerStateData;

    [Header("Combat")]
    public S_EnemyDodgeState dodgeStateData;
    public S_PlayerDetectedMoveState detectedPlayerMoveStateData;
    public S_EnemyStunState stunStateData;
    public S_EnemyDeadState deadStateData;

    private void OnEnable()
    {
        meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
