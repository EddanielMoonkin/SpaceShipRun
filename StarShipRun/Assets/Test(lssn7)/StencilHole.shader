Shader "Custom/StencilHole"
{   
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Geometry-1" "ForceNoShadowCasting" = "True" }
        LOD 200

        Stencil
        {
            Ref 11
            Comp Always
            Pass Replace
            /*Ref 12
            Comp Always
            Pass Replace*/
        }

        CGPROGRAM
        #pragma surface surf NoLighting alpha   
        
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        //sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
