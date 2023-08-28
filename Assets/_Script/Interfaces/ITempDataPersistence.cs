using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITempDataPersistence
{
    void SaveTempData(TempData data);
    void LoadTempData(TempData data);
}

public class TempData
{
    public Dictionary<string, bool> defeatedEnemies;

    public TempData()
    {
        defeatedEnemies = new();
    }
}
