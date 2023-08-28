using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Entity entity;

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
        GameObject obj = ObjectPoolManager.SpawnObject(entity.gameObject, transform.position, Quaternion.identity);
        EnemyManager.Instance.RegisterEnemy(obj, SceneName);
    }

    private void OnEnable()
    {
        entity.OnDefeated += HandleDefeated;
    }

    private void OnDisable()
    {
        entity.OnDefeated -= HandleDefeated;
    }

    private void HandleDefeated()
    {
        isDefeated = true;
    }
}
