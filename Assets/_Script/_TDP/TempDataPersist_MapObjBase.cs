using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base classes for the map objects that need to persist their state between scenes.
/// </summary>
public class TempDataPersist_MapObjBase : MonoBehaviour, ITempDataPersistence
{
    public bool isAddedID;
    public string ID;

    protected bool isActivated = false;

    protected virtual void Awake()
    {
        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveTempData(TempData data)
    {
        if (data.activatedMapObjects.ContainsKey(ID))
        {
            data.activatedMapObjects.Remove(ID);
        }
        data.activatedMapObjects.Add(ID, isActivated);
    }

    public void LoadTempData(TempData data)
    {
        data.activatedMapObjects.TryGetValue(ID, out isActivated);

        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }
}
