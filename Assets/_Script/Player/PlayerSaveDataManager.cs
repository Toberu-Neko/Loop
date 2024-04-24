using Eflatun.SceneReference;
using System;
using UnityEngine;
using UnityEngine.Localization;

public class PlayerSaveDataManager : MonoBehaviour, IDataPersistance
{
    // Teleport
    public static PlayerSaveDataManager Instance { get; private set; }
    public string RecentSavepointID { get; set; } = "";

    [SerializeField] private SceneReference startAnimScene;

    [SerializeField] private Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        player.OnDead += HandleDiedFirstTime;
    }

    private void OnDisable()
    {
        player.OnDead -= HandleDiedFirstTime;
    }

    private void Start()
    {
        GameManager.Instance.OnSavepointInteracted += HandleSavepointInteract;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSavepointInteracted -= HandleSavepointInteract;
    }

    private void HandleSavepointInteract(string id, LocalizedString name)
    {
        RecentSavepointID = id;
    }

    private void HandleDiedFirstTime()
    {
    }

    public void LoadData(GameData data)
    {
        data.savepoints.TryGetValue(data.lastInteractedSavepointID, out SavepointDetails details);

        if (data.gotoSavePoint)
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
        if(RecentSavepointID != "")
        {
            data.lastInteractedSavepointID = RecentSavepointID;
        }

        data.playerPosition = transform.position;
    }
}
