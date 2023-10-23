using UnityEngine;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

public class PressETeleportTrigger : MonoBehaviour
{
    [Header("這個物件一定要在綠框中")]
    [SerializeField] private Transform teleportPos;
    [SerializeField] private GameObject pressEObject;

    protected bool inRange;
    private SceneReference currentScene;
    private PlayerInputHandler inputHandler;
    private Collider2D playerCol;

    private void OnEnable()
    {
        inRange = false;
        pressEObject.SetActive(false);
    }

    private void Update()
    {
        if (inRange)
        {
            if(inputHandler == null)
            {
                Debug.LogError("PressETeleportTrigger: inputHandler is null, at " + gameObject.name);
                return;
            }

            if (inputHandler.InteractInput)
            {
                inputHandler.UseInteractInput();

                if (currentScene == null)
                {
                    Debug.LogError("PressETeleportTrigger: currentScene is null, at " + gameObject.name);
                    return;
                }

                if(playerCol == null)
                {
                    Debug.LogError("PressETeleportTrigger: playerCol is null, at " + gameObject.name);
                    return;
                }


                playerCol.transform.position = teleportPos.position;
                GameManager.Instance.HandleChangeScene(currentScene.Name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            pressEObject.SetActive(true);

            if(inputHandler == null)
            {
                playerCol = collision;
                inputHandler = collision.GetComponent<PlayerInputHandler>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            inRange = false;
            pressEObject.SetActive(false);
        }
    }

    public void SetCurrentScene(SceneReference scene)
    {
        currentScene = scene;
    }

    private void OnDrawGizmos()
    {
        if (!TryGetComponent<BoxCollider2D>(out var boxCollider))
            return;

        Gizmos.color = Color.red;

        Bounds bounds = boxCollider.bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

}
