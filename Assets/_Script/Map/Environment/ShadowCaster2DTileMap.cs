using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(PolygonCollider2D))]
public class ShadowCaster2DTileMap : MonoBehaviour
{
    [Space]
    [SerializeField]
    private bool selfShadows = true;

    private PolygonCollider2D tilemapCollider;

    static readonly FieldInfo meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly FieldInfo shapePathHashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo generateShadowMeshMethod = typeof(ShadowCaster2D)
                                    .Assembly
                                    .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
                                    .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);

    public void Generate()
    {
        DestroyAllChildren();

        tilemapCollider = GetComponent<PolygonCollider2D>();
        if (tilemapCollider == null)
        {
            Debug.LogError("No PolygonCollider2D found on this GameObject.");
            return;
        }

        for (int pathIndex = 0; pathIndex < tilemapCollider.pathCount; pathIndex++)
        {
            Vector2[] pathVertices = tilemapCollider.GetPath(pathIndex);
            if (pathVertices == null || pathVertices.Length < 3)
            {
                Debug.LogError($"PolygonCollider2D path {pathIndex} must have at least 3 vertices.");
                continue;
            }

            GameObject shadowCaster = new GameObject($"shadow_caster_{pathIndex}");
            shadowCaster.transform.parent = gameObject.transform;
            ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
            shadowCasterComponent.selfShadows = this.selfShadows;

            // 计算阴影物体的位置和大小
            Bounds bounds = new Bounds(pathVertices[0], Vector3.zero);
            for (int i = 1; i < pathVertices.Length; i++)
            {
                bounds.Encapsulate(pathVertices[i]);
            }

            shadowCaster.transform.position = bounds.center;
            shadowCaster.transform.localScale = new Vector3(bounds.size.x, bounds.size.y, 1f);

            Vector3[] testPath = new Vector3[pathVertices.Length];
            for (int i = 0; i < pathVertices.Length; i++)
            {
                testPath[i] = pathVertices[i];
            }

            shapePathField.SetValue(shadowCasterComponent, testPath);
            shapePathHashField.SetValue(shadowCasterComponent, Random.Range(int.MinValue, int.MaxValue));
            meshField.SetValue(shadowCasterComponent, new Mesh());
            generateShadowMeshMethod.Invoke(shadowCasterComponent, new object[] { meshField.GetValue(shadowCasterComponent), shapePathField.GetValue(shadowCasterComponent) });
        }
    }

    public void DestroyAllChildren()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ShadowCaster2DTileMap))]
public class ShadowCastersGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ShadowCaster2DTileMap generator = (ShadowCaster2DTileMap)target;
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        if (GUILayout.Button("Generate"))
        {

            generator.Generate();

        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Destroy All Children"))
        {

            generator.DestroyAllChildren();

        }
    }
#endif
}