using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E3_StateData", menuName = "Data/Entity Data/Enemies/E3 Human Blockable Data")]
public class E3_StateData : BaseEnemyStateData
{
    [Header("Movement")]
    public ED_EnemyIdleState idleStateData;
    public ED_EnemyGroundMoveState groundMoveStateData;
    public ED_EnemyPlayerDetectedState playerDetectedStateData;
    public ED_EnemyShieldMoveState shieldMoveStateData;

    [Header("Attack")]
    public ED_EnemyChargeState chargeStateData;
    public ED_EnemyMeleeAttackState meleeAttackStateData;
    public ED_EnemyLookForPlayerState lookForPlayerStateData;

    [Header("Combat")]
    public ED_EnemyStunState stunStateData;
    public ED_EnemyDeadState deadStateData;

    private void OnEnable()
    {
        meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
