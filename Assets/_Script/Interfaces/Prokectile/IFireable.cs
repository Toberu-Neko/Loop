using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void Init(Vector2 fireDirection, float speed, ProjectileDetails details);
    void Fire();


    void HandlePerfectBlock();
}
