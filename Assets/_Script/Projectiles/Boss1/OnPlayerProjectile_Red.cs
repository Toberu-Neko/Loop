using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerProjectile_Red : OnPlayerProjectileBase
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private Vector2 knockbackDirection = Vector2.one;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnAction += OnPlayerProjectile_Red_OnAction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnAction -= OnPlayerProjectile_Red_OnAction;
    }

    private void OnPlayerProjectile_Red_OnAction()
    {
        if(player.StateMachine.CurrentState == player.IdleState || player.StateMachine.CurrentState == player.CrouchIdleState)
        {
            ReturnToPool();
        }
        else
        {
            playerCombat.Damage(damage, transform.position + transform.right, false);
            playerCombat.Knockback(knockbackDirection, knockbackForce, transform.position + transform.right, false);
        }
        ReturnToPool();
    }

    protected override void Update()
    {
        base.Update();
    }
}
