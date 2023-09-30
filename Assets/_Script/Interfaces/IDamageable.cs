using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(float damageAmount, Vector2 damagePosition, bool blockable = true);

    void GotoKinematicState(float time = -1f);

    void GoToStunState();

    GameObject GetGameObject();
}
