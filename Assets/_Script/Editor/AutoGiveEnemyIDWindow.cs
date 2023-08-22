using System.Linq;
using UnityEditor;
using UnityEngine;

public class AutoGiveEnemyIDWindow : EditorWindow
{

    [MenuItem("Window/AutoGiveEnemyIDWindow")]
    static void CreateWindow()
    {
        EditorWindow.GetWindow<AutoGiveEnemyIDWindow>();
    }

    void OnHierarchyChange()
    {
        var addedObjects = Resources.FindObjectsOfTypeAll<Entity>()
                                    .Where(x => x.isAdded < 2);

        foreach (var item in addedObjects)
        {
            //if (item.isAdded == 0) early setup

            if (item.isAdded == 1)
            {
                item.ID = System.Guid.NewGuid().ToString();
            }
            item.isAdded++;
        }
    }
}