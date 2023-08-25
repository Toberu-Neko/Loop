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
        var addedObjects = Resources.FindObjectsOfTypeAll<Entity>()
                                    .Where(x => x.isAdded < 2);

        foreach (var item in addedObjects)
        {
            //if (item.isAdded == 0) early setup

            if (item.isAdded == 1)
            {
                Debug.Log("Change!");
                item.ID = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(item);
            }
            item.isAdded++;


        }
    }
}