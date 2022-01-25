// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Combine"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OldTex("OtherTex", 2D) = "white" {}
        _ArrowSize("arrowSize", Range(0, 3000)) = 1
        _ScreenWidth("screenWidth", Range(0, 3000)) = 1
        _ScreenHeight("screenHeight", Range(0, 3000)) = 1
        _Offset("offset", Range(0, 1)) = 1
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
            sampler2D _OldTex;
            sampler2D _MainTex;
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_OldTex, i.uv);
                fixed4 col2 = tex2D(_MainTex, i.uv);
                return col*col.a* ceil((col.r + col.g + col.b)/3) + col2*(1- col.a * ceil((col.r + col.g + col.b) / 3));
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
            sampler2D _OldTex;
            sampler2D _MainTex;
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_OldTex, i.uv);
                fixed4 col2 = tex2D(_MainTex, i.uv);
                return col2 * col2.a + col * (1 - col2.a);
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
            sampler2D _OldTex;
            sampler2D _MainTex;
            float _ArrowSize;
            float _ScreenWidth;
            float _ScreenHeight;
            float _Offset;
            fixed4 frag(v2f i) : SV_Target
            {
                float2 oldUV = float2(((i.uv.x - 0.5) * _ScreenWidth / _ArrowSize) + 0.5, ((i.uv.y - 0.5) * _ScreenHeight / _ArrowSize) + 0.5 - _Offset*2);
                fixed4 top = tex2D(_OldTex, oldUV);
                fixed4 bottom = tex2D(_MainTex, i.uv);
                return top * top.a + bottom * (1 - top.a);
            }
            ENDCG
        }
    }
}
