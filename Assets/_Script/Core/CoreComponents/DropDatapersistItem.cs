using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDatapersistItem : CoreComponent
{
    private Death death;
    [SerializeField] private GameObject item;

    protected override void Awake()
    {
        base.Awake();

        if(item == null)
        {
            Debug.LogError("DropDatapersistItem: item is null");
            this.enabled = false;
            return;
        }

        death = core.GetCoreComponent<Death>();
    }

    private void OnEnable()
    {
        death.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        death.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        GameObject obj = Instantiate(item, core.transform.position, Quaternion.identity);

        float dropForce = 10f;
        Vector2 dir = new(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
        Rigidbody2D rig = obj.GetComponent<Rigidbody2D>();
        rig.velocity = dir * dropForce;
    }
}
