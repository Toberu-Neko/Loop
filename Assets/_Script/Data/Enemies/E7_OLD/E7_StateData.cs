using UnityEngine;

[CreateAssetMenu(fileName = "E7_StateData", menuName = "Data/Entity Data/Enemies/E7 Static Ranged")]
public class E7_StateData : BaseEnemyStateData
{
    public ED_EnemyIdleState idleStateData;
    public ED_PlayerDetectedState playerDetectedState;
    public ED_EnemySnipingState snipingStateData;
}
