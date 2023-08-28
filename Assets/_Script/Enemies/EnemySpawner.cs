using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawner : MonoBehaviour, ITempDataPersistence
{
    public string ID;
    [HideInInspector] public int isAdded = 0;

    [SerializeField] private GameObject enemy;
    private Entity entity;
    [HideInInspector] public string SceneName { get; set;}
    private SpriteRenderer SR;
    private bool isDefeated = false;

    private void Awake()
    {
        if (isDefeated)
        {
            gameObject.SetActive(false);
            return;
        }

        SR = GetComponent<SpriteRenderer>();
        SR.enabled = false;
    }

    public void StartSpawning()
    {
        GameObject obj = ObjectPoolManager.SpawnObject(enemy, transform.position, Quaternion.identity);
        entity = obj.GetComponent<Entity>();
        entity.OnDefeated += HandleDefeated;
        EnemyManager.Instance.RegisterEnemy(obj, SceneName);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.OnDefeated -= HandleDefeated;
    }

    private void HandleDefeated()
    {
        Debug.Log("Enemy defeated: " + enemy.name);
        isDefeated = true;
    }

    public void SaveTempData(TempData data)
    {
        if (data.defeatedEnemies.ContainsKey(ID))
        {
            data.defeatedEnemies.Remove(ID);
        }
        data.defeatedEnemies.Add(ID, isDefeated);
    }

    public void LoadTempData(TempData data)
    {
        data.defeatedEnemies.TryGetValue(ID, out isDefeated);

        if (isDefeated)
        {
            gameObject.SetActive(false);
        }
    }
}
