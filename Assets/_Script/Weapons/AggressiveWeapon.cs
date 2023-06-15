using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    private List<IDamageable> detectedDamageable = new();

    protected override void Awake()
    {
        base.Awake();

        if(weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong weapon data type");
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }

    private void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];
        foreach(IDamageable damageable in detectedDamageable.ToList())
        {
            damageable.Damage(new AttackDetails(state.GetPlayerPosition(), details.damageAmount, 1f));
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        
        if(collision.TryGetComponent<IDamageable>(out var damageable))
        {
            detectedDamageable.Add(damageable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            detectedDamageable.Remove(damageable);
        }
    }
}
