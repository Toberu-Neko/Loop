using Cinemachine;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private BossBase boss;
    [SerializeField] private BossRoomCamLookat camLookat;
    [SerializeField] private GameObject bossRoomDoor;
    [SerializeField] private CinemachineVirtualCamera bossCamera;
    [SerializeField] private CinemachineVirtualCamera orgCamera;
    [SerializeField] private Collider2D col;

    [Header("BGM")]
    [SerializeField] private string normalBGMName;
    [SerializeField] private string bossBGMName;

    private float enterPosX;
    private bool interacted;
    private bool defeated;

    private void Awake()
    {
        bossRoomDoor.SetActive(false);
        interacted = false;
    }

    private void Start()
    {
        DataPersistenceManager.Instance.GameData.defeatedBosses.TryGetValue(boss.BossName, out defeated);

        if (defeated)
        {
            AudioManager.instance.PlayBGM(normalBGMName);
        }
        else
        {
            AudioManager.instance.StopBGM(normalBGMName, 1f);
        }
    }
    private void HandleBossDefeated()
    {
        bossRoomDoor.SetActive(false);
        CamManager.Instance.SwitchCamera(orgCamera);
        boss.Stats.Health.OnCurrentValueZero -= HandleBossDefeated;
        AudioManager.instance.StopBGM(bossBGMName, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enterPosX = (collision.transform.position - col.bounds.center).normalized.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interacted)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - col.bounds.center).normalized;
            if ((enterPosX < 0f && exitDirection.x < 0f)
                ||
                (enterPosX > 0f && exitDirection.x > 0f)
                ||
                (!boss.gameObject.activeInHierarchy))
            {
                return;
            }

            if (!defeated)
            {
                interacted = true;
                boss.Stats.Health.OnCurrentValueZero += HandleBossDefeated;
                AudioManager.instance.PlayBGM(bossBGMName);

                bossRoomDoor.SetActive(true);
                CamManager.Instance.SwitchCamera(bossCamera);
                camLookat.SetPlayer(collision.transform);
                boss.HandleEnterBossRoom();
                UI_Manager.Instance.ActiveBossUI(boss);
            }
        }
    }
}
