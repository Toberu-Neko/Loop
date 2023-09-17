using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyMonitor
{
    static List<string> IDs = new();
    static HierarchyMonitor()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    static void OnHierarchyChanged()
    {
        IDs = new();
        AssignUniqueIDs<EnemySpawner>();
        AssignUniqueIDs<BreakableWall>();
        AssignUniqueIDs<PickupTreasure>();

        CheckIfSame();
    }

    static void AssignUniqueIDs<T>() where T : MonoBehaviour, IUniqueID
    {
        var allObjects = GameObject.FindObjectsOfType<T>();

        foreach (var item in allObjects)
        {
            if (!Application.isPlaying)
            {
                var script = item.GetComponent<T>();

                if (script != null && !script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    IDs.Add(script.ID);
                    EditorUtility.SetDirty(item);
                }
            }
        }
    }

    static void CheckIfSame()
    {
        foreach (string item in IDs)
        {
            string targetValue = item;
            int count = IDs.Count(x => x == targetValue);

            if (count > 1)
            {
                Debug.LogError("Duplicate ID: " + item);
            }
        }
    }
}