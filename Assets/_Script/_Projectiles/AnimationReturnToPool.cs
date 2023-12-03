using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationReturnToPool : MonoBehaviour
{
    public void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
