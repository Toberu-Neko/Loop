using UnityEngine;

/// <summary>
/// This is the base class for all the components in the core.
/// </summary>
public class CoreComponent : MonoBehaviour
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
