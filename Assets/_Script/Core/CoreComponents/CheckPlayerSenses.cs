using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerSenses : CoreComponent
{
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform playerMinAgroCheck;
    [SerializeField] private Transform playerMaxAgroCheck;
    [SerializeField] private Transform playerCloseRangeCheck;
    [SerializeField] private Transform canSeePlayerCheck;

    [SerializeField] private Vector2 minAgroV2 = Vector2.one;
    [SerializeField] private Vector2 maxAgroV2 = Vector2.one;
    [SerializeField] private Vector2 closeRangeActionV2 = Vector2.one;

    public bool CanSeePlayer
    {
        get
        {
            if(!IsPlayerInMaxAgroRange)
            {
                return false;
            }

            Vector2 delta = IsPlayerInMaxAgroRange.point - (Vector2)transform.position;
            return !Physics2D.Raycast(canSeePlayerCheck.position, delta.normalized, delta.magnitude, whatIsGround);
        }
    }

    public bool IsPlayerInMinAgroRange
    {
        get
        {
            return Physics2D.BoxCast(playerMinAgroCheck.position, minAgroV2, 0f, transform.right, 0.1f, whatIsPlayer);
        }
    }

    public RaycastHit2D IsPlayerInMaxAgroRange
    {
        get
        {
            return Physics2D.BoxCast(playerMaxAgroCheck.position, maxAgroV2, 0f, transform.right, 0.1f, whatIsPlayer);
        }
    }

    public bool IsPlayerInCloseRangeAction
    {
        get
        {
            return Physics2D.BoxCast(playerCloseRangeCheck.position, closeRangeActionV2, 0f, transform.right, 0.1f, whatIsPlayer);
        }
    }

    public virtual void OnDrawGizmos()
    {
        if (playerMinAgroCheck && playerMaxAgroCheck && playerCloseRangeCheck)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(playerMinAgroCheck.position, minAgroV2);

            Gizmos.DrawWireCube(playerMaxAgroCheck.position, maxAgroV2);

            Gizmos.DrawWireCube(playerCloseRangeCheck.position, closeRangeActionV2);

        }
    }
}
