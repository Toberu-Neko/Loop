using UnityEngine;

public class EnemySpawner : MonoBehaviour, ITempDataPersistence
{
    public bool isAddedID;
    public string ID;

    [SerializeField] private GameObject enemy;
    [SerializeField] private FacingDir facingDir;
    private enum FacingDir
    {
        right,
        left
    }
    private Entity entity;
    
    [HideInInspector] public string SceneName { get; set;}
    [SerializeField] private SpriteRenderer SR;
    private bool isDefeated = false;

    private void Awake()
    {
        if (isDefeated)
        {
            gameObject.SetActive(false);
            return;
        }
        SR.enabled = false;
    }

    public void StartSpawning()
    {
        GameObject obj = ObjectPoolManager.SpawnObject(enemy, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
        entity = obj.GetComponent<Entity>();

        if(facingDir == FacingDir.left)
        {
            entity.SetFacingDirection(-1);
        }
        else
        {
            entity.SetFacingDirection(1);
        }

        entity.OnDefeated += HandleDefeated;
        EnemyManager.Instance.RegisterEnemy(obj, SceneName);
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
        if (data.defeatedObjects.ContainsKey(ID))
        {
            data.defeatedObjects.Remove(ID);
        }
        data.defeatedObjects.Add(ID, isDefeated);
    }

    public void LoadTempData(TempData data)
    {
        data.defeatedObjects.TryGetValue(ID, out isDefeated);

        if (isDefeated)
        {
            gameObject.SetActive(false);
        }
    }
}
