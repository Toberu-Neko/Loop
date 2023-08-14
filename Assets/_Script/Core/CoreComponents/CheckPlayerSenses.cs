using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerSenses : CoreComponent
{
    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] private Transform playerMinAgroCheck;
    [SerializeField] private Transform playerMaxAgroCheck;
    [SerializeField] private Transform playerCloseRangeCheck;

    [SerializeField] private Vector2 minAgroV2 = Vector2.one;
    [SerializeField] private Vector2 maxAgroV2 = Vector2.one;
    [SerializeField] private Vector2 closeRangeActionV2 = Vector2.one;


    public bool IsPlayerInMinAgroRange
    {
        get
        {
            return Physics2D.BoxCast(playerMinAgroCheck.position, minAgroV2, 0f, transform.right, 0.1f, whatIsPlayer);
        }
    }

    public bool IsPlayerInMaxAgroRange
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(playerMinAgroCheck.position, minAgroV2);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(playerMaxAgroCheck.position, maxAgroV2);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(playerCloseRangeCheck.position, closeRangeActionV2);

        }
    }
}
