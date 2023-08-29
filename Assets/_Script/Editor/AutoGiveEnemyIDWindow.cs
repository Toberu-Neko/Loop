using System.Linq;
using UnityEditor;
using UnityEngine;

public class AutoGiveEnemyIDWindow : EditorWindow
{

    [MenuItem("Window/Auto Enemy ID")]
    static void CreateWindow()
    {
        GetWindow<AutoGiveEnemyIDWindow>();
    }

    private void OnGUI()
    {
        GUILayout.Label("放置敵人時記得要開啟這個畫面", EditorStyles.label);
    }

    void OnHierarchyChange()
    {
        var spawnerObjects = Resources.FindObjectsOfTypeAll<EnemySpawner>()
                                    .Where(x => x.isAdded < 2);

        foreach (var item in spawnerObjects)
        {
            //if (item.isAdded == 0) early setup

            if (item.isAdded == 1)
            {
                item.ID = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(item);
            }
            item.isAdded++;
        }

        var entityObjs = Resources.FindObjectsOfTypeAll<Entity>()
                                    .Where(x => x.isAdded < 2);

        foreach (var item in entityObjs)
        {
            //if (item.isAdded == 0) early setup

            if (item.isAdded == 1)
            {
                item.ID = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(item);
            }
            item.isAdded++;
        }
    }
}