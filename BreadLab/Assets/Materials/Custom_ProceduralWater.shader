Shader "Custom/ProceduralWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveScale ("Wave Scale", Range(0.01, 0.2)) = 0.1
        _Speed ("Speed", Range(0.1, 2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float _WaveScale;
        float _Speed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv = IN.uv_MainTex;
            uv.x += _Speed * _Time.y;
            uv.y += _Speed * _Time.y;
            float wave = tex2D(_MainTex, uv).r * _WaveScale;
            o.Albedo = float3(wave, wave, wave);
        }
        ENDCG
    }
    FallBack "Diffuse"
}