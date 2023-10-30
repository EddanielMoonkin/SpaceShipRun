Shader "Custom/PlanetShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color1 ("Color", Color) = (1,1,1,1)
        _Color2 ("Color", Color) = (1,1,1,1)
        _Color3 ("Color", Color) = (1,1,1,1)

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Emission("Emission", Color) = (1,1,1,1)
        _Height ("Height", Range(-1,1)) = 0
        _Seed("Seed", Range(0,10000)) = 10

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        
        #pragma surface surf Lambert noforwardadd noshadow vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 color: COLOR;    
        };

        float4 _Emission;
        fixed4 _Color;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _Color3;
        float _Height;
        float _Seed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

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
            result = lerp(z0, z1, u.x) + (z2 - z0) * u.y * (1.0 - u.x) + (z3 - z1) *
            u.x * u.y;
            return result;
        }

        void vert(inout appdata_full v)
        {
            float height = noise(v.texcoord, 5) * 0.75 + noise(v.texcoord, 30) *
            0.125 + noise(v.texcoord, 50) * 0.125;
                
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 color = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float height = IN.color.r;
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
            o.Albedo = color.rgb;
            o.Alpha = color.a;
            o.Emission = _Emission.xyz;
        }
        ENDCG
    }
    FallBack "Diffuse"
}