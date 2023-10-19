using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // 讓腳本知道SR是甚麼，才可以更換圖片
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private float updateTime = 0.25f;

    // 用來存放圖片的陣列
    private Sprite[] sprites1;
    private Sprite[] sprites2;
    private Sprite[] sprites3;
    private Sprite[] sprites4;

    private int count;
    private float timer;

    private void Awake()
    {
        // 讀取圖片, Resources.LoadAll<Sprite>("資料夾名稱");
        sprites1 = Resources.LoadAll<Sprite>("1");
        sprites2 = Resources.LoadAll<Sprite>("2");
        sprites3 = Resources.LoadAll<Sprite>("3");
        sprites4 = Resources.LoadAll<Sprite>("4");

        count = 0;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateTime)
        {
            timer = 0;
            UpdateSprites();
        }
    }


    private void UpdateSprites()
    {
        // 用迴圈來更換圖片
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.sprite.name == "1")
            {
                spriteRenderer.sprite = sprites1[count];
            }
            else if (spriteRenderer.sprite.name == "2")
            {
                spriteRenderer.sprite = sprites2[count];
            }
            else if (spriteRenderer.sprite.name == "3")
            {
                spriteRenderer.sprite = sprites3[count];
            }
            else if (spriteRenderer.sprite.name == "4")
            {
                spriteRenderer.sprite = sprites4[count];
            }
        }
        count++;
    }

}
