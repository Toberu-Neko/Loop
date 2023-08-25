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
    [SerializeField] private SpriteRenderer SR;

    private Color color;

    public void SetPlayerSR(SpriteRenderer t_SR, int facingDir)
    {
        if (facingDir == -1)
        {
            this.SR.flipX = true;
        }
        else
        {
            this.SR.flipX = false;
        }

        this.SR.sprite = t_SR.sprite;
    }

    private void OnEnable()
    {
        alpha = alphaSet;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
