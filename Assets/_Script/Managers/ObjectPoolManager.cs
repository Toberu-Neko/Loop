using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    public static List<PooledObjectInfo> ObjectPools = new();

    private GameObject objectPoolEmptyHolder;

    private static GameObject particleSystemEmpty;
    private static GameObject projectileEmpty;
    private static GameObject gameObjects;
    private static GameObject enemies;
    private static GameObject none;

    public enum PoolType
    {
        ParticleSystem,
        Projectiles,
        GameObjects,
        Enemies,
        None
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            // Debug.Log("Found more than one object pool manager in the scene, delete the new one.");
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        objectPoolEmptyHolder = new GameObject("Pooled Object");

        particleSystemEmpty = new GameObject("Particle Effects");
        particleSystemEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        projectileEmpty = new GameObject("Projectiles");
        projectileEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        gameObjects = new GameObject("GameObjects");
        gameObjects.transform.SetParent(objectPoolEmptyHolder.transform);

        enemies = new GameObject("Enemies");
        enemies.transform.SetParent(objectPoolEmptyHolder.transform);

        none = new GameObject("None");
        none.transform.SetParent(objectPoolEmptyHolder.transform);

        DontDestroyOnLoad(objectPoolEmptyHolder);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if(pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name};
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObj == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            pool.ActiveObjects.Add(spawnableObj);

            if(parentObject != null)
                spawnableObj.transform.SetParent(parentObject.transform);
        }
        else
        {
            spawnableObj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObj);
            pool.ActiveObjects.Add(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, parentTransform);
            // pool.ActiveObjects.Add(spawnableObj);
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObj);
            // pool.ActiveObjects.Add(spawnableObj);
            spawnableObj.transform.SetParent(parentTransform);
            spawnableObj.transform.SetPositionAndRotation(parentTransform.position, parentTransform.rotation);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        if(obj == null)
        {
            Debug.Log("Object is null, can't return to pool.");
            return;
        }

        if (!obj.activeInHierarchy)
        {
            Debug.Log("Object is already inactive, can't return to pool.");
            return;
        }

        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if(pool == null)
        {
            Debug.Log("No pool found for " + goName);
            obj.SetActive(false);
            return;
        }
        else
        {
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
            pool.InactiveObjects.Add(obj);
            pool.ActiveObjects.Remove(obj);
        }
    }

    public static void ReturnAllObjectsToPool()
    {
        if(ObjectPools.Count == 0)
        {
            // Debug.LogWarning("No objects in pool to return.");
            return;
        }

        foreach(PooledObjectInfo pool in ObjectPools)
        {
            if(pool.ActiveObjects.Count == 0)
            {
                // Debug.LogWarning("No inactive objects in pool to return.");
                continue;
            }

            foreach(GameObject obj in pool.ActiveObjects.ToList())
            {
                if(obj == null)
                {
                    pool.ActiveObjects.Remove(obj);
                }
                else
                {
                    obj.SetActive(false);
                    pool.InactiveObjects.Add(obj);
                }
            }

            pool.ActiveObjects.Clear();
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        return poolType switch
        {
            PoolType.ParticleSystem => particleSystemEmpty,
            PoolType.Projectiles => projectileEmpty,
            PoolType.GameObjects => gameObjects,
            PoolType.Enemies => enemies,
            PoolType.None => none,
            _ => none,
        };
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new();
    public List<GameObject> ActiveObjects = new();
}
