using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float alphaSet = 0.8f;
    [SerializeField] private float alphaMultiplier = 0.85f;

    private Transform player;

    [SerializeField] private SpriteRenderer SR;
    private SpriteRenderer playerSR;

    private Color color;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        SR.sprite = playerSR.sprite;
        alpha = alphaSet;
        transform.SetPositionAndRotation(player.position, player.rotation);
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
