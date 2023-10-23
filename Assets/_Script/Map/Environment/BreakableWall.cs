using System;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IMapDamageableItem, ITempDataPersistence
{
    [SerializeField] protected Core core;
    protected Stats stats;
    protected Death death;
    protected ParticleManager particleManager;
    public bool isAddedID;
    public string ID;
    [SerializeField] private int health = 1;

    private bool isDefeated = false;
    protected event Action OnDefeated;

    protected virtual void Awake()
    {
        stats = core.GetCoreComponent<Stats>();
        death = core.GetCoreComponent<Death>();
        particleManager = core.GetCoreComponent<ParticleManager>();
    }

    public void LoadTempData(TempData data)
    {
        data.defeatedObjects.TryGetValue(ID, out isDefeated);

        if (isDefeated)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveTempData(TempData data)
    {
        if (data.defeatedObjects.ContainsKey(ID))
        {
            data.defeatedObjects.Remove(ID);
        }
        data.defeatedObjects.Add(ID, isDefeated);
    }

    public virtual void TakeDamage(float damage)
    {
        health --;
        if (health == 0)
        {
            OnDefeated?.Invoke();
            gameObject.SetActive(false);
            isDefeated = true;
        }
    }
}
