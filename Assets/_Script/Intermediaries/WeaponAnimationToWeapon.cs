using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour
{
    private OldWeapon weapon;
    private void Awake()
    {
        weapon = GetComponentInParent<OldWeapon>();
    }
    private void AnimationFinishTrigger()
    {
        weapon.AnimationFinishTrigger();
    }
    private void AnimationStartMovementTrigger()
    {
        weapon.AnimationStartMovementTrigger();
    }
    private void AnimationStopMovementTrigger()
    {
        weapon.AnimationStopMovementTrigger();
    }
    private void AnimationTurnOffFlipTrigger()
    {
        weapon.AnimationTurnOffFlipTrigger();
    }
    private void AnimationTurnOnFlipTrigger()
    {
        weapon.AnimationTurnOnFlipTrigger();
    }
    private void AnimationActionTrigger()
    {
        weapon.AnimationActionTrigger();
    }
}
