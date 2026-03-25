Shader "Custom/StarBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _BlueNoise ("Blue Noise", 2D) = "gray" {}
        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseSpeed ("Noise Speed", Vector) = (0,1,0,0)

        _Strength ("Shimmer Strength", Range(0,3)) = 1.0
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
            float _NoiseScale;
            float4 _NoiseSpeed;
            float _Strength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // base texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // scrolling noise
                float2 noiseUV = i.uv * _NoiseScale + _NoiseSpeed.xy * _Time.y;
                float n = tex2D(_BlueNoise, noiseUV).r;

                // full-range alpha control (can reach 0)
                float shimmer = lerp(1.0, n, _Strength);

                col.a *= shimmer;

                return col;
            }
            ENDCG
        }
    }
}