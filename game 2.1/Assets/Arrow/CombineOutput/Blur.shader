// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Blur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Size("OutlineSize", Range(0, 0.1)) = 0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            sampler2D _MainTex;
            half _Size;
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = 0;
                for (float index = 0; index < 10; index++) {
                    float2 uv = i.uv + float2(0, (index / 9 - 0.5) * _Size);
                    col += tex2D(_MainTex, uv);
                }
                return col/10;
            }
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            sampler2D _MainTex;
            half _Size;
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = 0;
                for (float index = 0; index < 10; index++) {
                    float2 uv = i.uv + float2((index / 9 - 0.5) * _Size, 0);
                    col += tex2D(_MainTex, uv);
                }
                return col / 10;
            }
            ENDCG
        }
    }
}
