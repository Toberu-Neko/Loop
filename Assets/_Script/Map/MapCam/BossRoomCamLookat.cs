using UnityEngine;

public class BossRoomCamLookat : MonoBehaviour
{
    [SerializeField] private Transform boss;
    private Transform player;

    [SerializeField] private bool changeFOV = true;
    [SerializeField] private float mivFOVDistance = 15f;
    [SerializeField] private float maxFOV = 120f;
    [SerializeField] private float devideFOV = 1.2f;

    private Vector2 workspace = new();
    private float orgFOV;


    private void Update()
    {
        if (player != null && boss.gameObject.activeInHierarchy) 
        {
            workspace = (player.position + boss.position) / 2;

            transform.position = Vector2.Lerp(transform.position, workspace, Time.deltaTime * 2f);

            if (changeFOV)
            {
                if (Vector2.Distance(player.position, boss.position) < mivFOVDistance)
                {
                    CamManager.Instance.CurrentCam.m_Lens.FieldOfView = Mathf.Lerp(CamManager.Instance.CurrentCam.m_Lens.FieldOfView, orgFOV, Time.deltaTime * 2f);
                }
                else
                {
                    if(orgFOV + Vector2.Distance(player.position, boss.position) / devideFOV < maxFOV)
                    {
                        CamManager.Instance.CurrentCam.m_Lens.FieldOfView = Mathf.Lerp(CamManager.Instance.CurrentCam.m_Lens.FieldOfView, orgFOV + Vector2.Distance(player.position, boss.position) / devideFOV, Time.deltaTime * 2f);
                    }
                    else
                    {
                        CamManager.Instance.CurrentCam.m_Lens.FieldOfView = Mathf.Lerp(CamManager.Instance.CurrentCam.m_Lens.FieldOfView, maxFOV, Time.deltaTime * 2f);
                    }
                }
            }
        }

    }

    public void SetPlayer(Transform player)
    {
        this.player = player;

        orgFOV = CamManager.Instance.CurrentCam.m_Lens.FieldOfView;
        workspace = (player.position + boss.position) / 2f;
    }
}
