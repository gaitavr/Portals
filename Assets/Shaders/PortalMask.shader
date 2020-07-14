Shader "Unlit/PortalMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InactiveColor("Inactive Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _InactiveColor;
            uniform int _DrawingFlag;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;
                fixed4 col = tex2D(_MainTex, uv) * _DrawingFlag + _InactiveColor * (1 - _DrawingFlag);
                return col;
            }
            ENDCG
        }
    }
}
