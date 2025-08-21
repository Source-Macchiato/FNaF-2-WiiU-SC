Shader "Custom/PanoramicEffect"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _CurveAmount("Curve Amount", Range(0, 1)) = 0.3
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CurveAmount;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float x = uv.x - 0.5;
                float scaleY = max(0.02, 1.0 + (1.0 - _CurveAmount - 1.0) * 4.0 * x * x);
                uv.y = uv.y * scaleY + (1.0 - scaleY) * 0.5;

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}