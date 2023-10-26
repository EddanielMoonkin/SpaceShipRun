Shader "Unlit/MultiPassShader"
{
    Properties
    {
        _Tex1("Texture1", 2D) = "white" {}
        _Color("Color", COLOR) = (0,0,0,1)
        _Alpha ("Alpha", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        //LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Tex1; //текстура1
            float4 _Tex1_ST;           

            struct v2f
            {
                float2 uv : TEXCOORD0; //UV координаты вершины
                float4 vertex : SV_POSITION; //координаты вершины
            };

            v2f vert(appdata_full v)
            {
                v2f result;                 
                result.vertex = UnityObjectToClipPos(v.vertex);
                result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
                return result;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_Tex1, i.uv);
                return color;
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _Color;
            float _Alpha;   
                        

            struct v2f
            {
                float4 clipPos: SV_POSITION;
            };

            v2f vert (appdata_full v)
            {
                v2f o;
                v.vertex = float4(0,0,0,0);
                v.vertex.xyz += v.normal * 0.55;
                o.clipPos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = _Color*_Alpha;
                return color;
            };
            ENDCG
        }
    }
}
