// The error was due to the redefinition of '_SpecColor'. It was defined once and then attempted to be defined again.
// The second definition has been removed to resolve the error.

Shader "Custom/RealisticWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _Color ("Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (1,1,1,1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _Reflectivity ("Reflectivity", Range (0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf BlinnPhong

        sampler2D _MainTex;
        sampler2D _BumpMap;
        float4 _Color;
        float4 _SpecColor;
        float _Shininess;
        float _Reflectivity;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 tex = tex2D(_MainTex, IN.uv_MainTex);
            float3 bump = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Albedo = _Color.rgb * tex.rgb;
            o.Normal = bump;
            o.Specular = _Shininess;
            o.Reflection = _Reflectivity;
            o.Specular = _SpecColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}