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
        var all = GameObject.FindObjectsOfType<EnemySpawner>();
        foreach (var spawner in all)
        {
            if (!Application.isPlaying)
            {
                EnemySpawner script = spawner.GetComponent<EnemySpawner>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(spawner);
                    // Debug.Log("Change spawner ID: " + spawner.name);
                }
            }
        }
    }
}