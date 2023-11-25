using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float updateRate = 0.02f;
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
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }

        SR.sprite = t_SR.sprite;
    }

    private void OnEnable()
    {
        alpha = alphaSet;
        timeActivated = Time.time;
        Invoke(nameof(SetAlpha), updateRate);
    }

    private void SetAlpha()
    {
        CancelInvoke(nameof(SetAlpha));
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;
        if(alpha > 0.01f)
        {
            Invoke(nameof(SetAlpha), updateRate);
        }
        else
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

}
