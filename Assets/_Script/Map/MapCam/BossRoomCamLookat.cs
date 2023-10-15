using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomCamLookat : MonoBehaviour
{
    [SerializeField] private Transform boss;
    private Transform player;

    private float mivFOVDistance = 15f;

    private Vector2 workspace = new();
    private float orgFOV;

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (player != null && boss.gameObject.activeInHierarchy) 
        {
            workspace = (player.position + boss.position) / 2;

            transform.position = workspace;


            if (Vector2.Distance(player.position, boss.position) < mivFOVDistance)
            {
                CamManager.Instance.CurrentCam.m_Lens.FieldOfView = Mathf.Lerp(CamManager.Instance.CurrentCam.m_Lens.FieldOfView, orgFOV, Time.deltaTime * 2f);
            }
            else
            {
                CamManager.Instance.CurrentCam.m_Lens.FieldOfView = Mathf.Lerp(CamManager.Instance.CurrentCam.m_Lens.FieldOfView, orgFOV + Vector2.Distance(player.position, boss.position) / 2f, Time.deltaTime * 5f);
            }
        }

    }

    public void SetPlayer(Transform player)
    {
        this.player = player;

        orgFOV = CamManager.Instance.CurrentCam.m_Lens.FieldOfView;
        workspace = (player.position + boss.position) / 2;
    }
}
