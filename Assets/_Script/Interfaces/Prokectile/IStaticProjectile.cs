using UnityEngine;

public interface IStaticProjectile
{
    void Init(Vector2 destination, float explodeTime);

    bool Exploded();
}
