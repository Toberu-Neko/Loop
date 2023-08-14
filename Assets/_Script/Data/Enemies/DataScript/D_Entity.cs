using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public LayerMask whatIsPlayer;

    [Tooltip("����|���|����")]
    public bool collideDamage = true;
    public WeaponAttackDetails collisionAttackDetails;
}
