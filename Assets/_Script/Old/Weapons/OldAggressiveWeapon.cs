using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OldAggressiveWeapon : OldWeapon
{
    protected SO_WeaponData_Sword aggressiveWeaponData;

    private List<IDamageable> detectedDamageables = new();
    private List<IKnockbackable> detectedKnockbackables = new();

    protected override void Awake()
    {
        base.Awake();

        if(weaponData.GetType() == typeof(SO_WeaponData_Sword))
        {
            aggressiveWeaponData = (SO_WeaponData_Sword)weaponData;
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
        WeaponAttackDetails details = aggressiveWeaponData.NormalAttackDetails[attackCounter];
        foreach(IDamageable damageable in detectedDamageables.ToList())
        {
            damageable.Damage(details.damageAmount, (Vector2)core.transform.parent.position);
        }

        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(details.knockbackAngle, details.knockbackForce, Movement.FacingDirection, (Vector2)core.transform.position);
        }

    }

    public void AddToDetected(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            detectedDamageables.Add(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            detectedKnockbackables.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            detectedDamageables.Remove(damageable);
        }

        if (collision.TryGetComponent<IKnockbackable>(out IKnockbackable knockbackable))
        {
            detectedKnockbackables.Remove(knockbackable);
        }
    }
}
