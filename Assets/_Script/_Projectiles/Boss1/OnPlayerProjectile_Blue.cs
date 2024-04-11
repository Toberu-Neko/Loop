using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerProjectile_Blue : OnPlayerProjectileBase
{
    [Header("Damage")]
    [SerializeField] private float damageDuration = 1.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float speedMultiplier = 0.4f;
    [SerializeField] private float damagePace = 0.5f;

    private bool startDamage;
    private float startMagicTime;
    private float orgStartMagicTime;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        startDamage = false;
        startMagicTime = 0f;

        OnAction += HandleAction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnAction -= HandleAction;
    }

    private void HandleAction()
    {
        if (player.StateMachine.CurrentState != player.IdleState && player.StateMachine.CurrentState != player.CrouchIdleState)
        {
            ReturnToPool();
            return;
        }
        else
        {
            startDamage = true;
            startMagicTime = 0f;
            orgStartMagicTime = Time.time;
            playerCombat.SetDebuffMultiplier(speedMultiplier, damageDuration);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (startDamage)
        {
            startMagicTime = stats.Timer(startMagicTime);
            orgStartMagicTime = stats.Timer(orgStartMagicTime);

            if (Time.time >= startMagicTime + damagePace && startDamage)
            {
                playerCombat.Damage(damage, transform.position + transform.right, false);
                startMagicTime = Time.time;
            }

            if(Time.time >= orgStartMagicTime + damageDuration)
            {
                ReturnToPool();
            }
        }
    }
}
