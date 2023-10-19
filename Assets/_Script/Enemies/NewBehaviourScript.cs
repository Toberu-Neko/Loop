using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // ���}�����DSR�O�ƻ�A�~�i�H�󴫹Ϥ�
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private float updateTime = 0.25f;

    // �ΨӦs��Ϥ����}�C
    private Sprite[] sprites1;
    private Sprite[] sprites2;
    private Sprite[] sprites3;
    private Sprite[] sprites4;

    private int count;
    private float timer;

    private void Awake()
    {
        // Ū���Ϥ�, Resources.LoadAll<Sprite>("��Ƨ��W��");
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
        // �ΰj��ӧ󴫹Ϥ�
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
