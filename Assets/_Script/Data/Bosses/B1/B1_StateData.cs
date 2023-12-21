using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "B1_StateData", menuName = "Data/Entity Data/Bosses/B1_StateData")]
public class B1_StateData : BaseEnemyStateData
{
    [Header("Normal")]
    public ED_EnemyIdleState idleStateData;
    public ED_PlayerDetectedMoveState playerDetectedMoveStateData;
    public ED_FlyingMovementState flyingMovementStateData;
    public ED_EnemyIdleState flyingIdleStateData;
    public ED_BackToIdleState backToIdleStateData;

    [Header("Attack")]
    public ED_EnemyRangedAttackState blueRangedAttackStateData;
    public ED_EnemyRangedAttackState redRangedAttackStateData;
    public ED_EnemyPerfectBlockState perfectBlockStateData;
    public ED_EnemyProjectiles counterAttackObjsData;

    [Header("Skill")]
    public ED_ChooseRandomBulletState chooseRandomBulletStateData;
    public ED_EnemyJumpAndMultiAttackState jumpAndMultiAttackStateData;
    public ED_FourSkyAttackState fourSkyAttackStateData;
    public ED_SliceRoomAndExplodeState sliceRoomAndExplodeStateData;
    public ED_AbovePlayerAttackState abovePlayerAttackStateData;

    [Header("Other")]
    public ED_EnemyStunState stunStateData;

    [Header("SFX")]
    public Sound block;
    
}
