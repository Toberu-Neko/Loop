using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//你可以发现我们的shader仅仅获取了这里计算得到的纹理偏移数据,非常的方便
//这说明我们要知道shader能做什么,不能做什么,合理利用CPU去计算再传给显卡去做
public class Wave : MonoBehaviour
{
    //波的尺寸
    public int waveWigth = 128;
    public int waveHeight = 128;
    [Range(0,1)]
    public float AttenuationRate = 0.01f;

    //波的数据:因为涉及到两个相互计算,所以需要两个数组
    float[,] waveA;
    float[,] waveB;
    //我们要记录并传递给着色器的纹理
    Texture2D tex_uv;
    //自身渲染器
    SpriteRenderer render;
    //计算边界
    private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        waveA = new float[waveWigth, waveHeight];
        waveB = new float[waveWigth, waveHeight];
        render = GetComponent<SpriteRenderer>();
        tex_uv = new Texture2D(waveWigth, waveHeight);
        render.material.SetTexture("_WaveTex", tex_uv);
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //我们要做的是鼠标滑到哪里我们在哪里做水波
        if (Input.GetMouseButton(0)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit)
            {
                Vector3 pos = new Vector3((hit.point.x - box.bounds.center.x) / box.bounds.size.x, (hit.point.y - box.bounds.center.y) / box.bounds.size.y, 0);
                int x = (int)((pos.x + 0.5f) * waveWigth);
                int y = (int)((pos.y + 0.5f) * waveHeight);
                PutDrop(x, y);
            }
        }
        ComputeWave();
    }

    /// <summary>
    /// 使用水波--这样做是要比PutASock更真实
    /// </summary>
    void PutDrop(int x, int y)
    {
        int radius = 20;
        float dis;
        for (int i = -radius; i <= radius; i++) 
        {
            for (int j = -radius; j <= radius; j++)
            {
                if (x + i >= 0 && x + i < waveWigth - 1 && y + j >= 0 && y + j < waveHeight - 1) 
                {
                    dis = Mathf.Sqrt(i * i + j * j);
                    if (dis < radius) 
                    {
                        waveA[x + i, y + j] = Mathf.Cos(dis / radius * Mathf.PI);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 产生波纹的核心代码
    /// </summary>
    void ComputeWave()
    {
        for (int w = 1; w < waveWigth - 1; w++)
        {
            for (int h = 1; h < waveHeight - 1; h++)
            {
                waveB[w, h] = (waveA[w - 1, h - 1] +
                    waveA[w - 1, h] +
                    waveA[w - 1, h + 1] +
                    waveA[w, h - 1] +
                    waveA[w, h + 1] +
                    waveA[w + 1, h - 1] +
                    waveA[w + 1, h] +
                    waveA[w + 1, h + 1]) / 4 - waveB[w, h];
                //限定值在-1到1之间
                float value = waveB[w, h];
                if (value > 1)
                {
                    waveB[w, h] = 1;
                }
                if (value < -1)
                {
                    waveB[w, h] = -1;
                }
                //UV偏移值
                float offset_u = (waveA[w - 1, h] - waveA[w + 1, h]) / 2;
                float offset_v = (waveA[w, h - 1] - waveA[w, h + 1]) / 2;
                //因为要转换为颜色值还需要限定到(0,1)
                float r = offset_u / 2 + 0.5f;
                float g = offset_v / 2 + 0.5f;
                //更新纹理--因为只有UV方向,所以b先为0
                tex_uv.SetPixel(w, h, new Color(r, g, 0));
                //纹理衰减--每次减自身的0.01--无限趋近于0--能量衰减
                waveB[w, h] -= waveB[w, h] * AttenuationRate;
            }
        }
        //应用改变
        tex_uv.Apply();
        //交换A,B数组
        float[,] tmpArr = waveA;
        waveA = waveB;
        waveB = tmpArr;


    }
}
