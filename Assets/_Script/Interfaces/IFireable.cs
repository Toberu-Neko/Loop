using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    void Fire(Vector2 fireDirection, ProjectileDetails details);
}
