Shader "Custom Shaders/TopoNoiseContours_World"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (1,1,1,1)

        // Height source (grayscale). Use a tiling noise/height texture.
        _HeightTex ("Height/Noise (R)", 2D) = "gray" {}

        // World-space mapping controls
        _WorldScale ("World Scale (tiling)", Range(0.001, 5)) = 0.05
        _WorldOffset ("World Offset (XY)", Vector) = (0,0,0,0)

        // Contour controls
        _LineDensity ("Line Density (lines per height unit)", Range(1, 200)) = 40
        _LineWidth ("Line Width (0..0.5 of interval)", Range(0.001, 0.25)) = 0.06

        // Height shaping
        _HeightStrength ("Height Strength", Range(0, 5)) = 1
        _HeightBias ("Height Bias", Range(-2, 2)) = 0

        // Optional: animate noise
        _ScrollSpeed ("Scroll Speed (XY)", Vector) = (0,0,0,0)

        // Optional: fade in/out by distance from world origin (XY plane)
        _FadeStart ("Fade Start Distance", Float) = 0
        _FadeEnd   ("Fade End Distance", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _LineColor;

            sampler2D _HeightTex;
            float _WorldScale;
            float4 _WorldOffset;

            float _LineDensity;
            float _LineWidth;

            float _HeightStrength;
            float _HeightBias;

            float4 _ScrollSpeed;

            float _FadeStart;
            float _FadeEnd;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Returns a hard contour line alpha from a continuous height value.
            float ContourAlphaHard(float h, float density, float widthFrac)
            {
                density = max(1.0, density);

                // h -> scaled height space where each integer boundary is a contour level
                float x = h * density;

                // fractional distance to nearest integer level in [0, 0.5]
                float f = frac(x);
                float d = min(f, 1.0 - f); // 0 at line center

                // widthFrac is in [0..0.5] of the interval, so compare against it directly
                float w = saturate(widthFrac) * 0.5;

                return step(d, w);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // World-space UV (XY plane), stable even if object/camera moves
                float2 uv = (i.worldPos.xy + _WorldOffset.xy) * _WorldScale;

                // Optional scrolling (still world-anchored + time drift if you want it)
                uv += _ScrollSpeed.xy * _Time.y;

                // Height from texture (0..1), then shape it
                float h = tex2D(_HeightTex, uv).r;
                h = (h + _HeightBias) * _HeightStrength;

                // Contour lines only (transparent background)
                float a = ContourAlphaHard(h, _LineDensity, _LineWidth);

                // Optional distance fade (if FadeEnd <= FadeStart or FadeEnd == 0, no fade)
                if (_FadeEnd > _FadeStart && _FadeEnd > 0.0)
                {
                    float dist = length(i.worldPos.xy); // centered on world origin
                    float t = saturate((dist - _FadeStart) / max(1e-4, (_FadeEnd - _FadeStart)));
                    // t = 0 near, 1 far; if you want fade-in instead, swap (1-t)
                    a *= (1.0 - t); // fade OUT as distance increases
                }

                fixed4 col = _LineColor;
                col.a *= a;
                return col;
            }
            ENDCG
        }
    }
}
