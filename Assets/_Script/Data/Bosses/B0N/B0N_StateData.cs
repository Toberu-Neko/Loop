using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "B0N_StateData", menuName = "Data/Entity Data/Bosses/B0N_StateData")]
public class B0N_StateData : BaseEnemyStateData
{
    public ED_EnemyIdleState idleStateData;
    public ED_PlayerDetectedMoveState detectedMoveStateData;

    [Header("Attack")]
    public ED_EnemyMeleeAttackState normalAttack1StateData;
    public ED_EnemyMeleeAttackState normalAttack2StateData;
    public ED_EnemyMeleeAttackState strongAttackStateData;

    [Header("Skill")]
    public ED_MultiAttackState MultiAttackState;
    public ED_EnemyChargeState ChargeState;

    [Header("Other")]
    public ED_EnemyStunState stunStateData;
}
