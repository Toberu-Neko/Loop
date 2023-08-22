using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    }

    public void SaveData(ref GameData data)
    {
        data.debugInputCount = DebugInputCount;
    }


}
