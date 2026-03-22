Shader "Custom/Particles/BlueNoiseDither_BuiltIn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color   ("Tint", Color) = (1,1,1,1)

        _BlueNoise ("Blue Noise (tileable)", 2D) = "gray" {}
        _NoiseTexSize ("Blue Noise Size (e.g. 128)", Float) = 128
        _NoiseScale ("Noise Tiling (screen-space)", Float) = 1.0

        _DitherStrength ("Dither Strength (alpha)", Range(0, 0.25)) = 0.06
        _DitherRGB ("Dither RGB (0/1)", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        // Typical particle transparency setup
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _BlueNoise;

            fixed4 _Color;
            float _NoiseTexSize;
            float _NoiseScale;
            float _DitherStrength;
            float _DitherRGB;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos      : SV_POSITION;
                float2 uv       : TEXCOORD0;
                fixed4 color    : COLOR;
                float4 screenPos: TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // Screen-space UV (0..1)
                float2 screenUV = (i.screenPos.xy / i.screenPos.w);

                // Convert to pixel coords (screen space)
                float2 px = screenUV * _ScreenParams.xy;

                // Blue-noise UV tiling in screen space:
                // (px / noiseSize) gives "noise texel space", then scale for larger/smaller pattern
                float2 noiseUV = (px / max(_NoiseTexSize, 1.0)) * _NoiseScale;

                // Sample blue noise (0..1) -> (-0.5..0.5)
                float n = tex2D(_BlueNoise, noiseUV).r - 0.5;

                // Dither alpha (main fix for transparency banding)
                col.a = saturate(col.a + n * _DitherStrength);

                // Optional: dither rgb too
                if (_DitherRGB > 0.5)
                {
                    col.rgb = saturate(col.rgb + n * _DitherStrength);
                }

                return col;
            }
            ENDCG
        }
    }

    FallBack Off
}
