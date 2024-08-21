using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    /// <summary>
    /// DataPersistance objects in base scene should load their data from the GameData object.
    /// Projects in level scenes should load their data from the GameData object in the base scene, using Start(), Awake() or OnEnable().
    /// </summary>
    /// <param name="data"></param>
    void LoadData(GameData data);

    /// <summary>
    /// Save all data in the active scene to the file, including the inactive objects and objects in the level scene.
    /// </summary>
    /// <param name="data"></param>
    void SaveData(GameData data);
}
