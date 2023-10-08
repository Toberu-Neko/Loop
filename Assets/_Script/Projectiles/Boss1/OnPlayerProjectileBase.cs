using System;
using TMPro;
using UnityEngine;

public class OnPlayerProjectileBase : MonoBehaviour
{
    [SerializeField] protected Core core;
    protected Stats stats;

    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float duration = 3f;
    [SerializeField] private TextMeshProUGUI countDownText;
    private Collider2D combatCol;

    protected Player player;
    protected Combat playerCombat;
    protected event Action OnAction;
    private float startTime;
    private bool actioned;

    protected virtual void Awake()
    {
        stats = core.GetCoreComponent<Stats>();
    }

    protected virtual void OnEnable()
    {
        startTime = Time.time;
        actioned = false;

        countDownText.text = duration.ToString("F1");

        combatCol = Physics2D.OverlapCircle(transform.position, 2f, whatIsPlayer);
        Debug.Log(transform.position);

        if(combatCol == null)
        {
            Debug.LogError("No player found in the range of the projectile.");
            ReturnToPool();
            return;
        }

        player = combatCol.GetComponentInParent<Player>();
        playerCombat = player.Core.GetCoreComponent<Combat>();
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Update()
    {
        startTime = stats.Timer(startTime);
        float remainTime = startTime + duration - Time.time;

        if(remainTime >= 0f)
        {
            countDownText.text = remainTime.ToString("F1");
        }

        if(remainTime <= 0f && !actioned)
        {
            actioned = true;
            OnAction?.Invoke();
        }
    }

    protected void ReturnToPool()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
