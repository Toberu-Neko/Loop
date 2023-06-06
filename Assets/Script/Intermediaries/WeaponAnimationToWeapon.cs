using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour
{
    private Weapon weapon;
    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }
    private void AnimationFinishTrigger()
    {
        weapon.AnimationFinishTrigger();
    }
}
