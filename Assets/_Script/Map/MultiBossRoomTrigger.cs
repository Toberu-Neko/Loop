using Cinemachine;
using UnityEngine;

public class MultiBossRoomTrigger : MonoBehaviour
{
    [SerializeField] private BossBase[] bosses;
    [SerializeField] private BossRoomCamLookat camLookat;
    [SerializeField] private GameObject bossRoomDoor;
    [SerializeField] private GameObject bossRoomExitDoor;
    [SerializeField] private CinemachineVirtualCamera bossCamera;
    [SerializeField] private CinemachineVirtualCamera orgCamera;
    [SerializeField] private Collider2D col;

    [Header("BGM")]
    [SerializeField] private string normalBGMName;
    [SerializeField] private string bossBGMName;

    private int defeatCount;

    private float enterPosX;
    private bool interacted;
    private bool defeated;

    private void Awake()
    {
        bossRoomDoor.SetActive(false);
        bossRoomExitDoor.SetActive(false);
        interacted = false;
    }

    private void Start()
    {
        foreach (var boss in bosses)
        {
            DataPersistenceManager.Instance.GameData.defeatedBosses.TryGetValue(boss.BossName, out bool _defeated);

            if (!_defeated)
            {
                defeated = false;
            }
        }

        if (defeated)
        {
            AudioManager.Instance.PlayBGM(normalBGMName);
        }
        else
        {
            AudioManager.Instance.StopBGM(normalBGMName, 1f);
            defeatCount = 0;
        }
    }

    private void HandleBossDefeated()
    {
        defeatCount++;

        if (defeatCount == bosses.Length)
        {
            HandleAllBossDefeated();
        }
    }
    private void HandleAllBossDefeated()
    {
        bossRoomDoor.SetActive(false);
        bossRoomExitDoor.SetActive(true);

        CamManager.Instance.SwitchCamera(orgCamera);
        foreach (var boss in bosses)
        {
            boss.Stats.Health.OnCurrentValueZero -= HandleBossDefeated;
        }
        AudioManager.Instance.StopBGM(bossBGMName, 1f);
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
                (defeated))
            {
                return;
            }

            if (!defeated)
            {
                interacted = true;
                foreach (var boss in bosses)
                {
                    boss.Stats.Health.OnCurrentValueZero += HandleBossDefeated;
                }
                AudioManager.Instance.PlayBGM(bossBGMName);

                bossRoomDoor.SetActive(true);
                CamManager.Instance.SwitchCamera(bossCamera);
                camLookat.SetPlayer(collision.transform);
                foreach (var boss in bosses)
                {
                    boss.gameObject.SetActive(true);
                    boss.HandleEnterBossRoom();
                }
                UI_Manager.Instance.ActiveMultiBossUI(bosses[0], bosses[1]);
            }
        }
    }
}
