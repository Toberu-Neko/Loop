using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for handling the loot pool of the enemy, randomly drop items from the loot pool when the enemy dies.
/// </summary>
public class LootPool : CoreComponent
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private List<LootItem> lootItems = new();

    private Death death;
    protected override void Awake()
    {
        base.Awake();

        death = core.GetCoreComponent<Death>();

        foreach (LootItem item in core.CoreData.lootItems)
        {
            if (item != null)
                lootItems.Add(item);
        }
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
        List<LootItem> droppedItems = GetDroppedItems();

        foreach(LootItem item in droppedItems)
        {
            for (int i = 0; i < item.amount; i++)
            {
                SpawnItem(core.transform.position, item);
            }
        }
    }
    private List<LootItem> GetDroppedItems()
    {
        List<LootItem> droppedItems = new();

        foreach (LootItem loot in lootItems)
        {
            float randomNum = UnityEngine.Random.Range(0f, 100f);
            if (randomNum <= loot.dropChance)
            {
                droppedItems.Add(loot);
            }
        }

        return droppedItems;
    }

    private void SpawnItem(Vector3 position, LootItem lootItem)
    {
        GameObject dropItem = ObjectPoolManager.SpawnObject(dropItemPrefab, position, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);

        float dropForce = 10f;
        Vector2 dir = new(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.5f, 1f));
        Rigidbody2D rig = dropItem.GetComponent<Rigidbody2D>();
        rig.velocity = dir * dropForce;

        dropItem.GetComponent<PickupItem>().itemSO = lootItem.lootdetails;
    }
}

[Serializable]
public class LootItem
{
    public SO_ItemsBase lootdetails;
    public int amount = 1;

    [Range(0f, 100f)]
    public float dropChance;
}
