using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private SpriteRenderer playerSR;

    private Queue<GameObject> availableObjects = new();

    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        GrowPool();
    }


    private void GrowPool()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) // Return to pool
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }

        GameObject instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
