using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void Init(float speed, ProjectileDetails details);
    void Fire(Vector2 fireDirection);


    void HandlePerfectBlock();
}
