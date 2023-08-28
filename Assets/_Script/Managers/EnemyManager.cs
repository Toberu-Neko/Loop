using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private List<EnemyData> enemyData;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        enemyData = new();
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

    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Unload Scene: " + scene.name);

        List<EnemyData> enemiesToRemove = new();

        foreach (var obj in enemyData)
        {
            Debug.Log(obj.sceneName + " is in EnemyManager");

            if (obj.sceneName == scene.name)
            {
                ObjectPoolManager.ReturnObjectToPool(obj.enemy);
                enemiesToRemove.Add(obj);
            }
        }

        foreach (var objToRemove in enemiesToRemove)
        {
            enemyData.Remove(objToRemove);
            Debug.Log(objToRemove.enemy.name + " is removed from EnemyManager");
        }
    }

    public void RegisterEnemy(GameObject enemy, string sceneName)
    {
        EnemyData enemyData = new(enemy, sceneName);
        this.enemyData.Add(enemyData);
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
