using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Entity Data")]
public class D_Entity : ScriptableObject
{
    public LayerMask whatIsPlayer;

    [Tooltip("����|���|����")]
    public bool collideDamage = true;
    public WeaponAttackDetails collisionAttackDetails;

    public float gravityScale = 8f;
    public Sound damagedSFX;
}
