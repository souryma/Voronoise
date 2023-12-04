Shader "Voronoi/Test SetGlobalVector"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
        #pragma exclude_renderers d3d11 gles
        
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        

        sampler2D _MainTex;
        // uniform int _RegionAmount;
        // uniform float4 _Centroid;
        // uniform float4 _Centroids[] = {
        //     {0.2f, 0.3f, 0.0f, 0.0f},
        //     {0.7f, 0.6f, 0.0f, 0.0f},
        //     {0.5f, 0.3f, 0.0f, 0.0f}
        // };
        
        struct Input
        {
            // float3 worldPos;
            float2 uv_MainTex;
        };
        
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // float2 coord = IN.worldPos.xz;
            // float3 color = {255.0f, 0.0f, 0.0f};
            float3 color = 0.5f;
            
            // for(int i = 0; i < 3; i++)
            // {
                // float2 refPoint = _Centroids[i].xy;
                // float2 refPoint = _Centroid.xy;
                // if(distance(refPoint, coord) < 1)
                // {
                //     color += 1.0f;
                // }
            // }
            
            // Albedo comes from a texture tinted by color
            // o.Albedo = color;
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
//    FallBack "Diffuse"
}
