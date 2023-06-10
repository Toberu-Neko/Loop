using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveToEnemyCollider : MonoBehaviour, IDamageable
{
    BasicEnemyController basicEnemyController;
    private void Awake()
    {
        basicEnemyController = transform.parent.GetComponent<BasicEnemyController>();
    }
    public void Damage(float damageAmount, float damagePosition)
    {
        // basicEnemyController.Damage(damageAmount, damagePosition);
        Debug.Log("AH");
    }
}
