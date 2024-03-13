Shader "Effect Achievement/CenterWave"
{
    Properties
    {
        _WaveColor("WaveColor", Color) = (1,1,1,1)
        _WaveCenter("WaveCenter", Vector) = (0,0,0,0)
        _WaveWidth("WaveWidth", float) = 5
        _WaveSpeed("WaveSpeed", float) = 2
        _WaveInterval("WaveInterval", float) = 2
        _WaveRange("WaveRange", float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _WaveColor;
            float3 _WaveCenter;
            float _WaveWidth;
            float _WaveSpeed;
            float _WaveInterval;
            float _WaveRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float length = distance(_WaveCenter, i.uv);
                float dis = length + _Time.y * _WaveSpeed;
                dis *= _WaveInterval;
                float wave = dis - floor(dis);
                wave = saturate(pow(wave, _WaveWidth)) * smoothstep(_WaveRange, 0, length);

                return float4(wave * _WaveColor);
            }
            ENDCG
        }
    }
}
