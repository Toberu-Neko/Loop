using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E4_StateData", menuName = "Data/Entity Data/Enemies/E4 Human Melee Normal")]
public class E4_StateData : BaseEnemyStateData
{
    [Header("Movement")]
    public ED_EnemyIdleState idleStateData;
    public ED_EnemyGroundMoveState groundMoveStateData;
    public ED_PlayerDetectedState playerDetectedStateData;

    [Header("Attack")]
    public ED_EnemyMeleeAttackState meleeAttackStateData;
    public ED_EnemyLookForPlayerState lookForPlayerStateData;

    [Header("Combat")]
    public ED_EnemyDodgeState dodgeStateData;
    public ED_PlayerDetectedMoveState detectedPlayerMoveStateData;
    public ED_EnemyStunState stunStateData;
    public ED_EnemyDeadState deadStateData;

    private void OnEnable()
    {
        meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
