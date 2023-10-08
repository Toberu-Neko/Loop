using UnityEngine;

public class WorldCanvasBase : MonoBehaviour
{
    protected Camera Cam { get; private set; }
    protected Canvas Canvas { get; private set; }

    protected virtual void Awake()
    {
        Canvas = GetComponent<Canvas>();
        Cam = Camera.main;
    }

    protected virtual void Update()
    {
        if(Cam != null)
        {
            if (transform.rotation != Cam.transform.rotation)
            {
                transform.rotation = Cam.transform.rotation;
            }
        }
    }

    protected virtual void OnEnable()
    {
        if(Cam == null)
        {
            Debug.LogError("No main camera found when onenable.");
            return;
        }
        Canvas.worldCamera = Cam;
    }

    protected virtual void OnDisable()
    {

    }
}
