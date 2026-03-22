Shader "Custom Shaders/MoonDotCull"
{
    Properties
    {
        _Color ("Moon Color", Color) = (1,1,1,1)
        _Phase ("Phase 0..1", Range(0,1)) = 0.5

        // rotation = lerp(215, 150, _Phase) * _RotMul
        _RotMul ("Rotation Multiplier", Range(0,3)) = 1

        // Blue-noise dithering
        _BlueNoise ("Blue Noise (R)", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Range(0.1, 50)) = 8

        // Gradient/quantization on lit side
        _LightSteps ("Light Steps", Range(1, 32)) = 8
        _LightMin   ("Light Min", Range(0,1)) = 0.15
        _LightMax   ("Light Max", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float  _Phase;
            float  _RotMul;

            sampler2D _BlueNoise;
            float  _NoiseScale;

            float _LightSteps;
            float _LightMin;
            float _LightMax;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos      : SV_POSITION;
                float3 normalVS : TEXCOORD0;
                float4 screenPos: TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normalVS = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Base light direction from phase (view space)
                float a = (_Phase + 0.5) * 6.28318530718; // 2*pi
                float3 L = float3(sin(a), 0.0, cos(a));

                // Phase-driven Z rotation
                float rotDeg = lerp(215.0, 150.0, _Phase) * _RotMul;
                float rz = radians(rotDeg);
                float s = sin(rz);
                float c = cos(rz);
                L = normalize(float3(
                    L.x * c - L.y * s,
                    L.x * s + L.y * c,
                    L.z
                ));

                float ndotl = dot(i.normalVS, L);

                // Cull dark side
                if (ndotl <= 0.0)
                    discard;

                // Screen-space UV (stable dithering)
                float2 suv = i.screenPos.xy / i.screenPos.w;

                // Blue-noise sample (0..1)
                float n = tex2D(_BlueNoise, suv * _NoiseScale).r;

                // Lit value 0..1
                float v = saturate(ndotl);

                // Map to gradient range (min..max) before quantization
                v = lerp(_LightMin, _LightMax, v);

                // Dithered quantization into steps (light side only)
                float steps = max(1.0, _LightSteps);
                float d = (n - 0.5) / steps;
                float q = floor(saturate(v + d) * steps) / steps;

                fixed4 col = _Color;
                col.rgb *= q;
                return col;
            }
            ENDCG
        }
    }
}
