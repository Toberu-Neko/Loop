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

        Debug.Log(data.lastInteractedSavepoint);

        if (details != null)
        {
            transform.position = details.teleportPosition;
        }
    }

    public void SaveData(GameData data)
    {
        if(RecentSavepointName != "")
        {
            data.lastInteractedSavepoint = RecentSavepointName;
        }
    }
}
