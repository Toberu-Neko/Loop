using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : CoreComponent
{
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private Transform spawnPoint;

    private Death death;
    private Combat combat;

    protected override void Awake()
    {
        base.Awake();

        death = core.GetCoreComponent<Death>();
        combat = core.GetCoreComponent<Combat>();
    }

    private void OnEnable()
    {
        combat.OnDamageAmount += ShowDamageNumber;
    }

    private void OnDisable()
    {
        combat.OnDamageAmount -= ShowDamageNumber;
    }

    private void ShowDamageNumber(float amount)
    {
        if(amount <= 0 || death.IsDead)
        {
            return;
        }

        GameObject obj = ObjectPoolManager.SpawnObject(damageNumberPrefab, spawnPoint.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);
        obj.GetComponent<DamageNum>().Init(amount);
    }
}
