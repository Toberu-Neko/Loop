using System;
using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour, IDataPersistance
{
    // Teleport
    public static PlayerSaveDataManager Instance { get; private set; }
    public string RecentSavepointName { get; set; } = "";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        GameManager.Instance.OnSavepointInteracted += HandleSavepointInteract;
    }

    private void HandleSavepointInteract(string name)
    {
        RecentSavepointName = name;
    }

    public void LoadData(GameData data)
    {
        data.savepoints.TryGetValue(data.lastInteractedSavepoint, out SavepointDetails details);

        if (data.interectWithSavePointThisSave)
        {
            if (details != null)
            {
                transform.position = details.teleportPosition;
            }
        }
        else
        {
            transform.position = data.playerPosition;
        }
    }

    public void SaveData(GameData data)
    {
        if(RecentSavepointName != "")
        {
            data.lastInteractedSavepoint = RecentSavepointName;
        }

        data.playerPosition = transform.position;
    }
}
