using System;
using System.Collections.Generic;
using UnityEngine;

public class LootPool : CoreComponent
{
    [SerializeField] private GameObject dropItemPrefab;
    private List<LootItem> lootItems = new();

    private Death death;
    protected override void Awake()
    {
        base.Awake();

        death = core.GetCoreComponent<Death>();

        lootItems = core.CoreData.lootItems;
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
        dropItem.GetComponent<SpriteRenderer>().sprite = lootItem.lootdetails.itemSprite;

        float dropForce = 10f;
        Vector2 dir = new(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.5f, 1f));
        Rigidbody2D rig = dropItem.GetComponent<Rigidbody2D>();
        rig.velocity = dir * dropForce;

        dropItem.GetComponent<PickupChip>().chipSO = lootItem.lootdetails;
    }
}

[Serializable]
public class LootItem
{
    public SO_Chip lootdetails;
    public int amount = 1;

    [Range(0f, 100f)]
    public float dropChance;
}
