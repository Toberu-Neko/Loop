using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTempParent : MonoBehaviour
{
    public static BaseTempParent Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }
}
