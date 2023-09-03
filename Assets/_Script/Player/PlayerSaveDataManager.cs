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
        if (data.playerPos != Vector3.zero)
            transform.position = data.playerPos;
    }

    public void SaveData(GameData data)
    {
        data.debugInputCount = DebugInputCount;
        data.playerPos = transform.position;
    }
}
