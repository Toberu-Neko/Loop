using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBallAttack : MonoBehaviour
{
    float timer = 0;
    [SerializeField] GameObject ball;
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 2f)
        {
            Instantiate(ball, transform);
            timer = 0;
        }
    }


}
