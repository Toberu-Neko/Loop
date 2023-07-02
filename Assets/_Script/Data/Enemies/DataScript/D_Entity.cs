using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public LayerMask whatIsPlayer;

    [Tooltip("¼²¨ì·|¤£·|¦©¦å")]
    public bool collideDamage = true;
    public WeaponAttackDetails collisionAttackDetails;

    [Tooltip("°»´ú¶ZÂ÷¡iµu¡j")]
    public float minAgroDistance = 3f;
    [Tooltip("°»´ú¶ZÂ÷¡iªø¡j")]
    public float maxAgroDistance = 4f;
    [Tooltip("°»´ú¶ZÂ÷¡iªñ¶ZÂ÷¡j")]
    public float closeRangeActionDistance = 1f;
}
