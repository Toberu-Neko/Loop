using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoffFrontTrigger : MonoBehaviour
{
    [SerializeField] private GameObject frontObj;

    private float alpha;
    private SpriteRenderer[] SRs;
    private List<Color> colors;
    private void Awake()
    {
        if(frontObj == null)
        {
            Debug.LogError("No obj in " + gameObject.name);
            gameObject.SetActive(false);
            return;
        }

        SRs = frontObj.GetComponentsInChildren<SpriteRenderer>();

        if(SRs == null)
        {
            Debug.LogError("No SR in children.");
            gameObject.SetActive(false);
            return;
        }

        colors = new();

        foreach (SpriteRenderer r in SRs)
        {
            colors.Add(r.color);
        }

        alpha = 1.0f;
    }

    private void TurnOff()
    {
        alpha = Mathf.Lerp(alpha, 0.0f, 0.2f);

        if (alpha < 0.05f)
        {
            alpha = 0;
        }

        for (int i = 0; i < SRs.Length; i++)
        {
            float r = SRs[i].color.r;
            float g = SRs[i].color.g;
            float b = SRs[i].color.b;

            SRs[i].color = new Color(r, g, b, alpha);
        }


        if (alpha > 0f)
        {
            Invoke(nameof(TurnOff), 0.05f);
        }
    }
    private void TurnOn()
    {
        alpha = Mathf.Lerp(alpha, 1.0f, 0.2f);

        if (alpha >= 0.95f)
        {
            alpha = 1f;
        }

        for (int i = 0; i < SRs.Length; i++)
        {
            float r = SRs[i].color.r;
            float g = SRs[i].color.g;
            float b = SRs[i].color.b;
            SRs[i].color = new Color(r, g, b, alpha);
        }


        if (alpha < 1f)
        {
            Invoke(nameof(TurnOn), 0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            alpha = 1.0f;
            TurnOff();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TurnOn();
        }
    }
}
