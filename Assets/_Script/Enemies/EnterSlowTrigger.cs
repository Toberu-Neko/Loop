using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterSlowTrigger : MonoBehaviour
{
    [SerializeField] private float slowMultiplier = 0.5f;
    private ISlowable playerSlowable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerSlowable = collision.GetComponent<ISlowable>();

            playerSlowable?.SetDebuffMultiplier(slowMultiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerSlowable?.SetDebuffMultiplierOne();
        }
    }
}
