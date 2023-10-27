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

    private float enterPosX;
    private bool interacted;

    private void Awake()
    {
        bossRoomDoor.SetActive(false);
        interacted = false;
    }
    private void HandleBossDefeated()
    {
        bossRoomDoor.SetActive(false);
        CamManager.Instance.SwitchCamera(orgCamera);
        boss.Stats.Health.OnCurrentValueZero -= HandleBossDefeated;
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

            DataPersistenceManager.Instance.GameData.defeatedBosses.TryGetValue(boss.BossName, out bool defeated);
            if (!defeated)
            {
                interacted = true;
                boss.Stats.Health.OnCurrentValueZero += HandleBossDefeated;

                bossRoomDoor.SetActive(true);
                CamManager.Instance.SwitchCamera(bossCamera);
                camLookat.SetPlayer(collision.transform);
                boss.HandleEnterBossRoom();
                UI_Manager.Instance.ActiveBossUI(boss);
            }
        }
    }
}
