using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "B1_StateData", menuName = "Data/Entity Data/Bosses/B1_StateData")]
public class B1_StateData : BaseEnemyStateData
{
    [Header("Normal")]
    public ED_EnemyIdleState idleStateData;
    public ED_PlayerDetectedMoveState playerDetectedMoveStateData;

    [Header("Attack")]
    public ED_EnemyRangedAttackState blueRangedAttackStateData;
    public ED_EnemyRangedAttackState redRangedAttackStateData;

    [Header("Skill")]
    public ED_ChooseRandomBulletState chooseRandomBulletStateData;
    public ED_EnemyJumpAndMultiAttackState jumpAndMultiAttackStateData;


    [Header("Other")]
    public ED_EnemyStunState stunStateData;
}
