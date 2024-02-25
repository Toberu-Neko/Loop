using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField, Range(0f,30f)] private float startDelay = 0f;
    [SerializeField] private float shootDelay = 2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private ProjectileDetails bulletDetails;

    private float startTime;
    private bool startShooting;

    private void Awake()
    {
        sr.enabled = false;
        startShooting = false;
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - startTime > startDelay && !startShooting)
        {
            startShooting = true;
            Shoot();
        }
    }

    private void Shoot()
    {
        IFireable fireable = ObjectPoolManager.SpawnObject(bulletPrefab, transform.position, transform.rotation, ObjectPoolManager.PoolType.Enemies).GetComponent<IFireable>();

        fireable.Init(bulletSpeed, bulletDetails);
        fireable.Fire(transform.right);
        Invoke(nameof(Shoot), shootDelay);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Shoot));
    }
}
