using System;
using UnityEngine;

public class EnterSceneTrigger : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;

    public event Action OnChangeSceneFinished;
    private void Start()
    {
        GameManager.Instance.RegisterEnterSceneTrigger(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(CamManager.instance.CurrentCam != cam)
            {
                CamManager.instance.SwitchCamera(cam);
                OnChangeSceneFinished?.Invoke();
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<BoxCollider2D>(out var boxCollider))
            return;

        Gizmos.color = Color.gray;

        Bounds bounds = boxCollider.bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
