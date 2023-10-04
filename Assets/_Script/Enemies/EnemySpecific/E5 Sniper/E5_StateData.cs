using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "E5_StateData", menuName = "Data/Entity Data/Enemies/E5 Sniper")]
public class E5_StateData : BaseEnemyStateData
{
    public ED_EnemyIdleState idleStateData;
    public ED_EnemySnipingState snipingStateData;
    public ED_EnemyStunState stunStateData;

    private void OnEnable()
    {
        // meleeAttackStateData.whatIsPlayer = base.whatIsPlayer;
    }
}
