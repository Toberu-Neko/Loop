using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E8_StateData", menuName = "Data/Entity Data/Enemies/E8 Flying Ranged")]
public class E8_StateData : BaseEnemyStateData
{
    public ED_EnemyIdleState idleStateData;
    public ED_FlyingMovementState movementStateData;
    public ED_ChooseSingleBulletState chooseSingleBulletStateData;
    public ED_EnemyRangedAttackState rangedAttackStateData;
    public ED_EnemyStunState stunStateData;
}
