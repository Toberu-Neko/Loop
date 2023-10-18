using System;
using UnityEngine;

public class EP_StaticBase : EnemyProjectile_Base, IStaticProjectile
{
    
    private Vector2 destination;
    private float startWaitingTime;
    private float explodeTime;
    protected State state;

    protected event Action OnExplodeAction;
    protected event Action OnStopAction;

    protected enum State
    {
        Moving,
        Waiting,
        Explode
    }

    protected override void Update()
    {
        base.Update();

        if (Vector2.Distance(transform.position, destination) < 1f && state == State.Moving)
        {
            state = State.Waiting;
            startWaitingTime = Time.time;
            OnStopAction?.Invoke();
        }

        if (state != State.Moving)
        {
            movement.SetVelocityZero();
        }

        if (state == State.Waiting)
        {
            startWaitingTime = stats.Timer(startWaitingTime);

            if (Time.time >= startWaitingTime + explodeTime)
            {
                OnExplodeAction?.Invoke();
                state = State.Explode;
            }
        }
    }

    public virtual void Init(Vector2 destination, float explodeTime)
    {
        this.destination = destination;
        this.explodeTime = explodeTime;
    }

    public override void Fire(Vector2 fireDirection, float speed, ProjectileDetails details)
    {
        base.Fire(fireDirection, speed, details);

        state = State.Moving;
    }
}
