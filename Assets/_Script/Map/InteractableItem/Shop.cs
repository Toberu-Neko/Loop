using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableMapItem_Base
{
    //serf give so
    protected override void OnEnable()
    {
        base.OnEnable();

        OnInteract += HandleOnInteract;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnInteract -= HandleOnInteract;
    }

    private void HandleOnInteract()
    {
        // UI Shop
        // Give SO
    }
}
