Shader "Custom/WorldDots"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Spacing ("Spacing", Float) = 2
        _Radius ("Radius", Float) = 0.2
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

            float4 _Color;
            float _Spacing;
            float _Radius;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 worldXY : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 world = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldXY = world.xy;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float spacing = max(_Spacing, 1e-4);

                // stable tiling (works in negative space)
                float2 grid = (frac(i.worldXY / spacing) - 0.5) * spacing;

                float dist = length(grid);

                // hard circle
                float circle = step(dist, _Radius);

                return float4(_Color.rgb, _Color.a * circle);
            }
            ENDCG
        }
    }
}