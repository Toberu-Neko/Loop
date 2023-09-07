using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_StateData : BaseEnemyStateData
{
    [Header("Normal")]
    public S_EnemyIdleState idleStateData;
    public S_EnemyPlayerDetectedState playerDetectedStateData;
    public S_PlayerDetectedMoveState playerDetectedMoveStateData;

    [Header("Skills")]
    public S_EnemyChargeState chargeStateData;
    public S_EnemyBookmarkState bookmarkStateData;

    [Header("Attacks")]
    public S_EnemyMeleeAttackState normalAttackStateData;
    public S_EnemyMeleeAttackState strongAttackStateData;
    public S_EnemyMeleeAttackState multiAttackStateData;
    public S_EnemyRangedAttackState rangedAttackStateData;

    [Header("Other")]
    public S_EnemyStunState stunStateData;
    public S_EnemyDeadState deadStateData;
}