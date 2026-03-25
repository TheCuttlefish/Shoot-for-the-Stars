Shader "Custom/ScrollingFade_BlueNoise"

{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _ScrollOffset ("Scroll Offset", Vector) = (0,0,0,0)

        _FadeTop ("Fade Top", Range(0,10)) = 1
        _FadeBottom ("Fade Bottom", Range(0,10)) = 1

        // Blue noise
        _BlueNoise ("Blue Noise (tileable)", 2D) = "gray" {}
        _NoiseTexSize ("Noise Size", Float) = 128
        _NoiseScale ("Noise Scale", Float) = 1.0
        _DitherStrength ("Dither Strength", Range(0,0.25)) = 0.06
        _DitherRGB ("Dither RGB", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _BlueNoise;

            float4 _Color;
            float4 _ScrollOffset;

            float _FadeTop;
            float _FadeBottom;

            float _NoiseTexSize;
            float _NoiseScale;
            float _DitherStrength;
            float _DitherRGB;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvRaw : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uvRaw = v.uv;

                // use manual scroll instead of _Time
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) + _ScrollOffset.xy;

                o.screenPos = ComputeScreenPos(o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // ===== EXTENDED FADE =====
                float fadeBottom = smoothstep(0.0, 1.0, i.uvRaw.y / max(_FadeBottom, 0.0001));
                float fadeTop = smoothstep(0.0, 1.0, (1.0 - i.uvRaw.y) / max(_FadeTop, 0.0001));
                float fade = fadeBottom * fadeTop;

                col.a *= fade;

                // ===== BLUE NOISE DITHER =====
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 px = screenUV * _ScreenParams.xy;

                float2 noiseUV = (px / max(_NoiseTexSize, 1.0)) * _NoiseScale;

                float n = tex2D(_BlueNoise, noiseUV).r - 0.5;

                col.a = saturate(col.a + n * _DitherStrength);

                if (_DitherRGB > 0.5)
                {
                    col.rgb = saturate(col.rgb + n * _DitherStrength);
                }

                return col;
            }
            ENDCG
        }
    }
}