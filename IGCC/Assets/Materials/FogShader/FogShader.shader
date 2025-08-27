Shader "Custom Post-Processing/Fog"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _FogColour("Fog Colour", Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}

        Pass {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float _distance;
            float _density;
            float _power;
            uniform float4 _FogColour;
            

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag(v2f i) : SV_TARGET {
                float4 tex = tex2D(_MainTex, i.uv);
                float4 depthTex = tex2D(_CameraDepthTexture, i.uv);
                float depth = depthTex.x;
                depth = saturate(depth * _distance);
                depth = saturate(pow(depth, _density));
                return lerp(tex, _FogColour, 1 - depth);
            }
            ENDHLSL
        }
    }
}
