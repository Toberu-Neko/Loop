using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyMonitor
{
    static HierarchyMonitor()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }


    static void OnHierarchyChanged()
    {
        var allSpawners = GameObject.FindObjectsOfType<EnemySpawner>();
        foreach (var item in allSpawners)
        {
            if (!Application.isPlaying)
            {
                EnemySpawner script = item.GetComponent<EnemySpawner>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }

        var allWalls = GameObject.FindObjectsOfType<BreakableWall>();

        foreach (var item in allWalls)
        {
            if (!Application.isPlaying)
            {
                BreakableWall script = item.GetComponent<BreakableWall>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }

        var allTreasures = GameObject.FindObjectsOfType<PickupTreasure>();

        foreach (var item in allTreasures)
        {
            if (!Application.isPlaying)
            {
                PickupTreasure script = item.GetComponent<PickupTreasure>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }

        var allMapObj = GameObject.FindObjectsOfType<DataPersistMapObjBase>();

        foreach (var item in allMapObj)
        {
            if (!Application.isPlaying)
            {
                DataPersistMapObjBase script = item.GetComponent<DataPersistMapObjBase>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }

        var allTempObj = GameObject.FindObjectsOfType<TempDataPersist_MapObjBase>();

        foreach (var item in allTempObj)
        {
            if (!Application.isPlaying)
            {
                TempDataPersist_MapObjBase script = item.GetComponent<TempDataPersist_MapObjBase>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }
    }
}