using UnityEngine;

public class MoneyDrop : CoreComponent
{
    private Death death;
    private int dropAmount = 1;
    [SerializeField] private GameObject moneyPrefab;

    protected override void Awake()
    {
        base.Awake();

        death = core.GetCoreComponent<Death>();
        dropAmount = core.CoreData.dropAmount;
    }

    private void OnEnable()
    {
        death.OnDeath += HandleDropMoney;
    }

    private void OnDisable()
    {
        death.OnDeath -= HandleDropMoney;
    }

    private void HandleDropMoney()
    {
        for (int i = 0; i < dropAmount; i++)
        {
            GameObject dropItem = ObjectPoolManager.SpawnObject(moneyPrefab, transform.position, Quaternion.identity);
            float dropForce = 10f;
            Vector2 dir = new(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
            Rigidbody2D rig = dropItem.GetComponent<Rigidbody2D>();
            rig.velocity = dir * dropForce;
        }
    }

}
