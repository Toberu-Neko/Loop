using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> coreComponents = new();

    private void Awake()
    {
    }

    public void LogicUpdate()
    {
        foreach (CoreComponent compent in coreComponents)
        {
            compent.LogicUpdate();
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

        if (comp == null)
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");

        return null;
    }
}
