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
                    // Debug.Log("Change spawner ID: " + spawner.name);
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
                    // Debug.Log("Change spawner ID: " + spawner.name);
                }
            }
        }
    }
}