using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDP_DropTrap : TempDataPersist_MapObjBase
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ProjectileDetails details;
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            GameObject obj = ObjectPoolManager.SpawnObject(trapPrefab, spawnPoint.position, Quaternion.identity);

            IFireable fireable = obj.GetComponent<IFireable>();

            fireable.Init(Vector2.down, details.speed, details);
            fireable.Fire();
            gameObject.SetActive(false);
        }
    }
}
