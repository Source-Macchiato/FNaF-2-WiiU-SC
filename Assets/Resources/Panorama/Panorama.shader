Shader "Custom/Panorama" 
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", Vector) = (1, 1, 0, 0)
        _Variables("Variables", Vector) = (0, 0, 0, 0)
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

                sampler2D _MainTex;
                float2 _PixelSize;
                float4 _Variables;

                v2f vert(appdata v) 
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target 
                {
                    float fB;
                    float fC;
                    float2 posTex;
                    float4 color = float4(0.0, 0.0, 0.0, 1.0);

                    int pDir = int(_Variables.x);
                    int zoom = int(_Variables.y);
                    int noWrap = int(_Variables.z);

                    if (pDir == 0) 
                    {
                        fB = 1.0 - (zoom * _PixelSize.y);
                        fC = max(0.02, 1.0 + (fB - 1.0) * 4.0 * pow((i.uv.x - 0.5), 2.0));
                        posTex = i.uv * float2(1.0, fC) + float2(0.0, (1.0 - fC) / 2.0);

                        if (noWrap == 0 || (posTex.y >= 0.0 && posTex.y <= 1.0)) 
                        {
                            color = tex2D(_MainTex, posTex);
                        }
                    }
                    else 
                    {
                        fB = 1.0 - (zoom * _PixelSize.x);
                        fC = max(0.05, 1.0 + (fB - 1.0) * 4.0 * pow((i.uv.y - 0.5), 2.0));
                        posTex = i.uv * float2(fC, 1.0) + float2((1.0 - fC) / 2.0, 0.0);

                        if (noWrap == 0 || (posTex.x >= 0.0 && posTex.x <= 1.0))
                        {
                            color = tex2D(_MainTex, posTex);
                        }
                    }

                    return color;
                }

                ENDCG
            }
        }
}
