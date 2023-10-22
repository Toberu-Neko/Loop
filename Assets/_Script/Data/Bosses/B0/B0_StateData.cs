using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "B0_StateData", menuName = "Data/Entity Data/Bosses/B0_StateData")]
public class B0_StateData : BaseEnemyStateData
{
    [Header("Normal")]
    public ED_EnemyIdleState idleStateData;
    public ED_PlayerDetectedState playerDetectedStateData;
    public ED_PlayerDetectedMoveState playerDetectedMoveStateData;

    [Header("Skills")]
    public ED_EnemyChargeState chargeStateData;
    public ED_EnemyBookmarkState bookmarkStateData;

    [Header("Attacks")]
    public ED_EnemyMeleeAttackState normalAttackStateData;
    public ED_EnemyMeleeAttackState strongAttackStateData;
    public ED_EnemyMeleeAttackState multiAttackStateData;
    public ED_EnemyRangedAttackState rangedAttackStateData;

    [Header("Other")]
    public ED_EnemyStunState stunStateData;
    public ED_EnemyDeadState deadStateData;

    private void OnEnable()
    {
        normalAttackStateData.whatIsPlayer = base.whatIsPlayer;
        strongAttackStateData.whatIsPlayer = base.whatIsPlayer;
        multiAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}