using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private List<EnemyData> enemyData;

    private TempData tempData;
    private List<ITempDataPersistence> tempDataPersistences;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        tempData = new();
        enemyData = new();
        tempDataPersistences = new();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        tempDataPersistences = FindAllTempDataObjs();
        LoadTempData();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveTempData();

        // Return all enemies to the pool when unloaded
        List<EnemyData> enemiesToRemove = new();

        foreach (var obj in enemyData)
        {
            if (obj.sceneName == scene.name)
            {
                ObjectPoolManager.ReturnObjectToPool(obj.enemy);
                enemiesToRemove.Add(obj);
            }
        }

        foreach (var objToRemove in enemiesToRemove)
        {
            enemyData.Remove(objToRemove);
            // Debug.Log(objToRemove.enemy.name + " is removed from EnemyManager");
        }
    }

    public void ResetTempData()
    {
        tempData = new();
        SaveTempData();
    }

    private void LoadTempData()
    {
        foreach (var obj in tempDataPersistences)
        {
            obj.LoadTempData(tempData);
        }
    }

    private void SaveTempData()
    {
        foreach (var obj in tempDataPersistences)
        {
            obj.SaveTempData(tempData);
        }
    }

    public void RegisterEnemy(GameObject enemy, string sceneName)
    {
        EnemyData enemyData = new(enemy, sceneName);
        this.enemyData.Add(enemyData);
    }

    private List<ITempDataPersistence> FindAllTempDataObjs()
    {
        IEnumerable<ITempDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>(true).OfType<ITempDataPersistence>();

        return new List<ITempDataPersistence>(dataPersistences);
    }

    private class EnemyData
    {
        public GameObject enemy;
        public string sceneName;

        public EnemyData(GameObject enemy, string sceneName)
        {
            this.enemy = enemy;
            this.sceneName = sceneName;
        }
    }
}
public class TempData
{
    public Dictionary<string, bool> defeatedObjects;
    public Dictionary<string, bool> activatedMapObjects;

    public TempData()
    {
        defeatedObjects = new();
        activatedMapObjects = new();
    }
}
