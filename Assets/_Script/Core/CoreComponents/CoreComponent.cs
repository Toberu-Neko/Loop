using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Core core;

    protected virtual void Awake()
    {
        if (!transform.parent.TryGetComponent<Core>(out core))
        {
            Debug.LogError("CoreComponent: Core not found!");
        }

        core.AddCompent(this);
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void LateLogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }
}
