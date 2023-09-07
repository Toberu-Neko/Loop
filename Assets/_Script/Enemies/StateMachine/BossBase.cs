using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : Entity, IDataPersistance
{
    [SerializeField] private string bossName;
    public override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnDefeated += HandleDefeated;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnDefeated -= HandleDefeated;
    }

    protected override void Start()
    {
        base.Start();

        if(bossName != "")
        {
            if (DataPersistenceManager.Instance.GameData.defeatedBosses.TryGetValue(bossName, out bool defeated))
            {
                if (defeated)
                {
                    //TODO: Spawn a chest or something
                    gameObject.SetActive(false);
                }
            }
        }
    }

    protected void HandleDefeated()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        Debug.LogWarning("BossData Shouldn't be loaded in this function");
    }

    public void SaveData(GameData data)
    {
        if(bossName == "")
        {
            Debug.LogError("Boss name is empty, boss data not saved. Object: " + gameObject.name);
            return;
        }

        if (data.defeatedBosses.ContainsKey(bossName))
        {
            data.defeatedBosses.Remove(bossName);
        }

        data.defeatedBosses.Add(bossName, true);
    }
}
