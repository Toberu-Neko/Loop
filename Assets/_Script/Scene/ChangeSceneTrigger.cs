using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using Cinemachine;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] private ChangeSceneDirection changeSceneDirection;

    [SerializeField] private Transform teleport_LeftOrUp;
    [SerializeField] private Transform teleport_RightOrDown;

    [Header("LR")]
    [SerializeField] private SceneReference leftScene;
    [SerializeField] private SceneReference rightScene;

    [Header("UD")]
    [SerializeField] private SceneReference upScene;
    [SerializeField] private SceneReference downScene;

    private Collider2D col;
    private bool isUnloaded = false;

    public event Action OnChangeSceneGoLeft;
    public event Action OnChangeSceneGoRight;
    public event Action OnChangeSceneGoUp;
    public event Action OnChangeSceneGoDown;

    private float enterPosX;
    private float enterPosY;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void Start()
    {
        GameManager.Instance.RegisterChangeSceneTrigger(this);

        if(changeSceneDirection == ChangeSceneDirection.LeftRight)
        {
            if(leftScene == null || rightScene == null)
            {
                Debug.LogError("ChangeSceneTrigger: LeftRight scene is null, at " + gameObject.name);
            }
        }
        else if(changeSceneDirection == ChangeSceneDirection.UpDown)
        {
            if (upScene == null || downScene == null)
            {
                Debug.LogError("ChangeSceneTrigger: UpDown scene is null, at " + gameObject.name);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enterPosX = (collision.transform.position - col.bounds.center).normalized.x;
            enterPosY = (collision.transform.position - col.bounds.center).normalized.y;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - col.bounds.center).normalized;
            if (changeSceneDirection == ChangeSceneDirection.LeftRight)
            {
                if (exitDirection.x > 0 && enterPosX < 0 &&
                    SceneManager.GetSceneByName(leftScene.Name).isLoaded)
                {
                    if (!isUnloaded)
                    {
                        UnloadScene(leftScene.Name);
                        OnChangeSceneGoRight?.Invoke();
                        collision.transform.position = teleport_RightOrDown.position;
                    }
                }
                else if (enterPosX > 0 && exitDirection.x < 0 &&
                    SceneManager.GetSceneByName(rightScene.Name).isLoaded)
                {
                    if (!isUnloaded)
                    {
                        UnloadScene(rightScene.Name);
                        OnChangeSceneGoLeft?.Invoke();
                        collision.transform.position = teleport_LeftOrUp.position;
                    }
                }
            }

            else if(changeSceneDirection == ChangeSceneDirection.UpDown)
            {
                if (exitDirection.y > 0 && enterPosY < 0 &&
                     SceneManager.GetSceneByName(downScene.Name).isLoaded)
                {
                    if (!isUnloaded)
                    {
                        UnloadScene(downScene.Name);
                        OnChangeSceneGoUp?.Invoke();
                        collision.transform.position = teleport_LeftOrUp.position;
                    }
                }
                else if (enterPosY > 0 && exitDirection.y < 0 &&
                      SceneManager.GetSceneByName(upScene.Name).isLoaded)
                {
                    if (!isUnloaded)
                    {
                        UnloadScene(upScene.Name);
                        OnChangeSceneGoDown?.Invoke();
                        collision.transform.position = teleport_RightOrDown.position;
                    }
                }
            }
            isUnloaded = false;
        }
    }

    private void UnloadScene(string sceneName)
    {
        isUnloaded = true;
        SceneManager.UnloadSceneAsync(sceneName);
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<BoxCollider2D>(out var boxCollider))
            return;

        Gizmos.color = Color.red;

        Bounds bounds = boxCollider.bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    private enum ChangeSceneDirection
    {
        LeftRight,
        UpDown
    }
}
/*
[System.Serializable]
public class ChangeSceneTriggerObjects
{
    public ChangeSceneDirection changeSceneDirection;
    [HideInInspector] public SceneReference leftScene;
    [HideInInspector] public SceneReference rightScene;
    [HideInInspector] public SceneReference upScene;
    [HideInInspector] public SceneReference downScene;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChangeSceneTrigger))]
public class ChangeSceneTriggerEditor : Editor
{
    ChangeSceneTrigger changeSceneTrigger;

    private void OnEnable()
    {
        changeSceneTrigger = (ChangeSceneTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(changeSceneTrigger.ChangeSceneTriggerObjects.changeSceneDirection == ChangeSceneTriggerObjects.ChangeSceneDirection.LeftRight)
        {
            SerializedObject serializedObject = new SerializedObject(changeSceneTrigger);
            SerializedProperty leftSceneProperty = serializedObject.FindProperty("ChangeSceneTriggerObjects.leftScene");
            EditorGUILayout.PropertyField(leftSceneProperty, new GUIContent("Left Scene"));
            serializedObject.ApplyModifiedProperties();

            changeSceneTrigger.ChangeSceneTriggerObjects.rightScene = null;
        }
        else
        {
            changeSceneTrigger.ChangeSceneTriggerObjects.upScene = null;
            changeSceneTrigger.ChangeSceneTriggerObjects.downScene = null;
        }
    }
}
#endif
*/