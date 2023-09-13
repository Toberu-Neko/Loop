using System;
using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour, IDataPersistance
{
    private PlayerInputHandler playerInputHandler;

    public int DebugInputCount { get; private set; } = 0;

    public event Action OnDebugInputCountChanged;

    private bool debugInput;
    private void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        debugInput = playerInputHandler.DebugInput;

        if (debugInput)
        {
            debugInput = false;
            DebugInputCount++;
            OnDebugInputCountChanged?.Invoke();
        }
    }

    public void LoadData(GameData data)
    {
        DebugInputCount = data.debugInputCount;

        data.savepoints.TryGetValue(data.lastInteractedSavepoint, out SavepointDetails details);

        Debug.Log("Load player position " + data.lastInteractedSavepoint);

        if (details != null)
        {
            Debug.Log("Teleport player to " + details.teleportPosition);
            transform.position = details.teleportPosition;
        }
    }

    public void SaveData(GameData data)
    {
        data.debugInputCount = DebugInputCount;
        data.playerPos = transform.position;
    }
}
