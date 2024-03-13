Shader "Effect Achievement/ProgramWave"
{
    properties
    {
        _MainTex("MainTex", 2D) = ""{}
    }

    SubShader
    {
        pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "unitycg.cginc"

            sampler2D _MainTex; 
            sampler2D _WaveTex;

            struct v2f
            {
                float4 pos:POSITION;
                float2 uv:TEXCOORD0;

            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;

                return o;
            }

            fixed4 frag(v2f IN) : COLOR
            {
                float2 uv = tex2D(_WaveTex, IN.uv).xy; //通过纹理颜色值获取uv
                uv = uv * 2 - 1; //(0,1)转回(-1,1);
                uv *= 0.05;
                IN.uv += uv;
                float4 mainColor = tex2D(_MainTex, IN.uv);

                return mainColor;
            }

            ENDCG
        }
    }
}