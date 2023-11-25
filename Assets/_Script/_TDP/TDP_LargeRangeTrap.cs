using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDP_LargeRangeTrap : TempDataPersist_MapObjBase
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private ProjectileDetails details;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private int spawnCount;
    [SerializeField] private float xSpawnDistance;

    [SerializeField] private Transform teleportPos;

    private Vector2[] spawnPositions;
    private EnemyProjectile_Damage[] spawnedTraps;
    private Collider2D playerCol;
    protected override void Awake()
    {
        base.Awake();

        spawnPositions = new Vector2[spawnCount];
        spawnedTraps = new EnemyProjectile_Damage[spawnCount];

        float allDistance = xSpawnDistance * (spawnCount - 1);
        Vector2 firstPos = new(spawnPos.position.x - allDistance / 2, spawnPos.position.y);

        for (int i = 0; i < spawnCount; i++)
        {
            spawnPositions[i] = new Vector2(firstPos.x + xSpawnDistance * i, firstPos.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            playerCol = collision;
            CamManager.Instance.CameraShake(3f);
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject obj = ObjectPoolManager.SpawnObject(trapPrefab, spawnPositions[i], Quaternion.identity);
                spawnedTraps[i] = obj.GetComponent<EnemyProjectile_Damage>();
                spawnedTraps[i].OnHitPlayer += HandleHitPlayer;

                spawnedTraps[i].Init(details.speed, details);
                spawnedTraps[i].Fire(Vector2.down);
            }

            isActivated = true;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            if (spawnedTraps[i] != null)
            {
                spawnedTraps[i].OnHitPlayer -= HandleHitPlayer;
            }
        }
    }

    private void HandleHitPlayer()
    {
        UI_Manager.Instance.BlockPlayerSight();
        playerCol.transform.position = teleportPos.position;
        isActivated = false;
        Reset();
    }

    private void Reset()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            spawnedTraps[i].OnHitPlayer -= HandleHitPlayer;
        }

        spawnedTraps = new EnemyProjectile_Damage[spawnCount];
    }

}
