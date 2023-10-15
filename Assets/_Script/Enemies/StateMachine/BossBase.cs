using System;
using UnityEngine;

public class BossBase : Entity, IDataPersistance
{
    [Header("Boss Basic")]
    [SerializeField, Range(-1, 1), Tooltip("1R, -1L")] private int initFacingPos = 1;
    [field: SerializeField] public string BossName { get; private set;}
    protected event Action OnEnterBossRoom;
    private bool defeated = false;
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

        movement.CheckIfShouldFlip(initFacingPos);
        if(BossName != "")
        {
            if (DataPersistenceManager.Instance.GameData.defeatedBosses.TryGetValue(BossName, out defeated))
            {
                if (defeated)
                {
                    //TODO: Spawn a opened chest or something
                    gameObject.SetActive(false);
                }
            }

            // DataPersistenceManager.Instance.AddDataPersistenceObj(this);
        }
    }

    protected void HandleDefeated()
    {
        defeated = true;
        DataPersistenceManager.Instance.SaveGame();
    }

    public void HandleEnterBossRoom()
    {
        OnEnterBossRoom?.Invoke();
    }

    public void LoadData(GameData data)
    {
        Debug.LogWarning("BossData Shouldn't be loaded in this function");
    }

    public void SaveData(GameData data)
    {
        if(BossName == "")
        {
            Debug.LogError("Boss name is empty, boss data not saved. Object: " + gameObject.name);
            return;
        }

        if (data.defeatedBosses.ContainsKey(BossName))
        {
            data.defeatedBosses.Remove(BossName);
        }

        data.defeatedBosses.Add(BossName, defeated);
    }
}
