using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaminaDamageable
{
    void TakeStaminaDamage(float damageAmount, Vector2 damagePosition, bool blockable = true);
}
