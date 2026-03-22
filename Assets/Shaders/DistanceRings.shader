Shader "Custom Shaders/DistanceRings_Solid"
{
    Properties
    {
        _MajorColor ("Major Color", Color) = (1,1,1,1)
        _MinorColor ("Minor Color", Color) = (1,1,1,1)

        _MajorSpacing ("Major Spacing (world units)", Float) = 10
        _MinorPerMajor ("Minor Lines Per Major (float)", Float) = 5

        _MajorWidth ("Major Width (world units)", Float) = 0.35
        _MinorWidth ("Minor Width (world units)", Float) = 0.12
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

            float4 _MajorColor;
            float4 _MinorColor;

            float _MajorSpacing;
            float _MinorPerMajor;
            float _MajorWidth;
            float _MinorWidth;

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

            float DistToNearestMultiple(float dist, float spacing)
            {
                spacing = max(spacing, 1e-4);
                float f = frac(dist / spacing);
                return min(f, 1.0 - f) * spacing; // distance in world units to nearest ring
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Center at world origin on XY plane
                float dist = length(i.worldPos.xy);

                float majorSpacing = max(_MajorSpacing, 1e-4);

                // --- MAJOR RINGS (SOLID / HARD EDGE) ---
                float dMajor = DistToNearestMultiple(dist, majorSpacing);
                float aMajor = step(dMajor, max(_MajorWidth, 1e-4)); // hard cutoff, no gradient

                // --- MINOR RINGS (SOLID / HARD EDGE) ---
                float aMinor = 0.0;
                float minorPerMajor = max(0.0, _MinorPerMajor);

                if (minorPerMajor > 0.0)
                {
                    // If you want N minor lines between majors, spacing is major/(N+1)
                    float minorSpacing = majorSpacing / (minorPerMajor + 1.0);

                    float dMinor = DistToNearestMultiple(dist, minorSpacing);
                    float a = step(dMinor, max(_MinorWidth, 1e-4)); // hard cutoff

                    // Suppress minors near major rings (hard suppression)
                    float suppress = step(max(_MajorWidth, 1e-4) * 1.5, dMajor);

                    aMinor = a * suppress;
                }

                // Composite (majors override minors)
                fixed4 col = fixed4(0,0,0,0);
                col = lerp(col, _MinorColor, saturate(aMinor));
                col = lerp(col, _MajorColor, saturate(aMajor));

                return col;
            }
            ENDCG
        }
    }
}
