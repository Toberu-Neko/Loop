using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// The core of the game object. It can automatically add all the core components to the game object, and can get the core components.
/// </summary>
public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> coreComponents = new();

    [field: SerializeField] public CoreData CoreData { get; private set; }

    public void LogicUpdate()
    {
        foreach (CoreComponent compent in coreComponents)
        {
            compent.LogicUpdate();
        }
    }

    public void LateLogicUpdate()
    {
        foreach (CoreComponent compent in coreComponents)
        {
            compent.LateLogicUpdate();
        }
    }

    public void PhysicsUpdate()
    {
        foreach (CoreComponent compent in coreComponents)
        {
            compent.PhysicsUpdate();
        }
    }

    public void AddCompent(CoreComponent compent)
    {
        if(!coreComponents.Contains(compent))
            coreComponents.Add(compent);
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = coreComponents.OfType<T>().FirstOrDefault();

        if (comp)
            return comp;

        
        comp = GetComponentInChildren<T>();

        if (comp)
            return comp;

        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");

        return null;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
