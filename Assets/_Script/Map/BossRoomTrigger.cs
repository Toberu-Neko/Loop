using Cinemachine;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private BossBase boss;
    [SerializeField] private GameObject bossRoomDoor;
    [SerializeField] private CinemachineVirtualCamera bossCamera;
    [SerializeField] private Collider2D col;

    private float enterPosX;

    private void OnEnable()
    {
        bossRoomDoor.SetActive(false);
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
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - col.bounds.center).normalized;
            if ((enterPosX < 0f && exitDirection.x < 0f)
                ||
                (enterPosX > 0f && exitDirection.x > 0f))
            {
                return;
            }

            DataPersistenceManager.Instance.GameData.defeatedBosses.TryGetValue(boss.BossName, out bool defeated);
            if (!defeated)
            {
                bossRoomDoor.SetActive(true);
                // TODO: Change camera
                CamManager.Instance.SwitchCamera(bossCamera);
                boss.HandleEnterBossRoom();

            }
        }
    }
}
