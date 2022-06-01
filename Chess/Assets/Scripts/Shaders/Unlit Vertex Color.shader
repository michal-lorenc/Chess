Shader "Lorenc/Unlit Vertex Color"
{
    Properties
    {
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}

        Pass
        {
            
            LOD 100
            Cull off
            Blend SrcAlpha OneMinusSrcAlpha
            Zwrite off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = i.color;
                return col;
            }
            ENDCG
        }



    }
}
