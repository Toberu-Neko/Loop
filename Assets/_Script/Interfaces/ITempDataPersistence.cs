using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITempDataPersistence
{
    void SaveTempData(TempData data);
    void LoadTempData(TempData data);
}

