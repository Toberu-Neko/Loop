using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SO_WeaponData : ScriptableObject
{
    public int AmountOfAttacks { get; protected set; }
    public float[] MovementSpeed { get;protected set; }
}
