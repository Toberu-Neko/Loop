using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBattle>().ApplyDamage(2, transform.position);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
