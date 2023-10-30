Shader "Unlit/PlanetUnlit"
{
    Properties
    {
        _Color1 ("Color", Color) = (1,1,1,1)
        _Color2 ("Color", Color) = (1,1,1,1)
        _Color3 ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        //_Emission("Emission", Color) = (1,1,1,1)
        _Height ("Height", Range(-1,1)) = 0
        _Seed("Seed", Range(0,10000)) = 10

        _Color("Color", COLOR) = (0,0,0,1)
        _Alpha ("Alpha", Range(0,1)) = 0.3        
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; //UV координаты вершины
                float4 vertex : SV_POSITION; //координаты вершины
                half4 color : COLOR;
            };

            fixed4 _Color;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            float _Height;
            float _Seed;
            float4 _MainTex_ST;
            sampler2D _MainTex;

            float hash(float2 st) 
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise(float2 p, float size)
            {
                float result = 0;
                p *= size;
                float2 i = floor(p + _Seed);
                float2 f = frac(p + _Seed / 739);
                float2 e = float2(0, 1);
                float z0 = hash((i + e.xx) % size);
                float z1 = hash((i + e.yx) % size);
                float z2 = hash((i + e.xy) % size);
                float z3 = hash((i + e.yy) % size);
                float2 u = smoothstep(0, 1, f);
                result = lerp(z0, z1, u.x) + (z2 - z0) * u.y * (1.0 - u.x) + (z3 - z1) * u.x * u.y;

                return result;
            }

            v2f vert(appdata v)
            {
                v2f result;
                result.vertex = UnityObjectToClipPos(v.vertex);
                result.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float height = noise(v.uv, 5) * 0.75 
                    + noise(v.uv, 30) * 0.125 + noise(v.uv, 50) * 0.125;
                result.color.r = height + _Height;
                return result;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);
                float height = i.color.r;
                if (height < 0.45)
                {
                    color = _Color1;
                }
                else if (height < 0.75)
                {
                    color = _Color2;
                }
                else
                {
                    color = _Color3;
                }
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
