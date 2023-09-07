using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E5_StateData", menuName = "Data/Entity Data/E5 Sniper")]
public class E5_StateData : BaseEnemyStateData
{
    public S_EnemyIdleState idleStateData;
    public S_EnemySnipingState snipingStateData;
    public S_EnemyStunState stunStateData;
    public S_EnemyDeadState deadStateData;

    private void OnEnable()
    {
        // meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
