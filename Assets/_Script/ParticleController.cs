using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        FinishAnim();
    }
    private void FinishAnim()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
