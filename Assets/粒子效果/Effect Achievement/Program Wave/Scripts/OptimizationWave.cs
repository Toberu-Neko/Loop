using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class OptimizationWave : MonoBehaviour
{
    [Range(1, 4)]
    public int waveDownSample = 4;
    public int waveRadius = 15;
    [Range(0, 0.1f)] public float waveStrength = 0.05f;
    [Range(0, 1)] public float AttenuationRate = 0.01f;

    //波的数据:因为涉及到两个相互计算,所以需要两个数组
    private int waveWigth;
    private int waveHeight;
    private float[,] waveA;
    private float[,] waveB;
    //因为Unity规定仅主线程可以调用SetPixel,所以我们在自己开的线程中先存值,在主线程中调用即可
    private Color[] colors;
    //我们要记录并传递给着色器的纹理
    private Texture2D tex_uv;
    //程序是否在运行
    private bool isRun = true;
    //线程睡眠时间
    private int sleepTimeMS;

    private SpriteRenderer render;
    private BoxCollider2D box;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始化记录波纹数组
        waveWigth = Screen.width / waveDownSample;
        waveHeight = Screen.height / waveDownSample;
        waveA = new float[waveWigth, waveHeight];
        waveB = new float[waveWigth, waveHeight];
        tex_uv = new Texture2D(waveWigth, waveHeight);
        colors = new Color[waveWigth * waveHeight];

        // 初始化水波材质
        render.material.SetTexture("_WaveTex", tex_uv);
        render.material.SetFloat("_WaveStrength", waveStrength);

        // 开启新线程进行水波计算
        Thread thread = new Thread(new ThreadStart(ComputeWave));
        thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        sleepTimeMS = (int)(Time.deltaTime * 1000);
        tex_uv.SetPixels(colors);
        tex_uv.Apply();

        //我们要做的是鼠标滑到哪里我们在哪里做水波
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit)
            {
                Vector3 pos = new Vector3(
                    (hit.point.x - box.bounds.center.x) / box.bounds.size.x, 
                    (hit.point.y - box.bounds.center.y) / box.bounds.size.y, 
                    0);
                int x = (int)((pos.x + 0.5f) * waveWigth);
                int y = (int)((pos.y + 0.5f) * waveHeight);
                PutDrop(x, y);
            }
        }
    }

    /// <summary>
    /// 使用水波
    /// </summary>
    void PutDrop(int x, int y)
    {
        int radius = waveRadius * waveRadius;
        int dis = 0;
        for (int i = -waveRadius; i <= waveRadius; i++)
        {
            for (int j = -waveRadius; j <= waveRadius; j++)
            {
                if (x + i >= 0 && x + i < waveWigth - 1 && y + j >= 0 && y + j < waveHeight - 1)
                {
                    dis = i * i + j * j;
                    if (dis < radius)
                    {
                        waveA[x + i, y + j] = Mathf.Cos(dis * Mathf.PI / radius);
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
        while (isRun)
        {
            //这样的循环会随着waveWigt和waveHeight的上升效率大大降低 -- 我们可以让ComputeWave在新的线程中进行
            //如果你打开status面板对比就知道了
            //我们还需要关闭Project Settings >> Quality >> Other >> VSync Count --垂直同步
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
                    colors[w + h * waveWigth] = new Color(r, g, 0);
                    //纹理衰减--每次减自身的0.01--无限趋近于0--能量衰减
                    waveB[w, h] -= waveB[w, h] * AttenuationRate;
                }
            }
            //交换A,B数组
            float[,] tmpArr = waveA;
            waveA = waveB;
            waveB = tmpArr;

            Thread.Sleep(sleepTimeMS);
        }

    }

    private void OnDestroy()
    {
        isRun = false;
    }
}
