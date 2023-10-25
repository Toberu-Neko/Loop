using UnityEngine;

[CreateAssetMenu(fileName = "E6_StateData", menuName = "Data/Entity Data/Enemies/E6 Broken Robot")]
public class E6_StateData : BaseEnemyStateData
{
    public ED_EnemyIdleState idleStateData;
    public ED_PlayerDetectedState playerDetectedState;
    public ED_PlayerDetectedMoveState detectedPlayerMoveStateData;
}
