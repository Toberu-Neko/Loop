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

    [Tooltip("�����Z���i�u�j")]
    public float minAgroDistance = 3f;
    [Tooltip("�����Z���i���j")]
    public float maxAgroDistance = 4f;
    [Tooltip("�����Z���i��Z���j")]
    public float closeRangeActionDistance = 1f;
}
