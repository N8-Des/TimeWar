Shader "Custom/FogOfWarShader"
{
    Properties
    {
        _FogMap ("FogMap", 2D) = "white" {}
        _DesaturationAmount ("Desaturation Amount", Range(0, 1)) = 0.8
        _FeatherAmount ("Feather Amount", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200
        
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _FogTexture;
            float _DesaturationAmount;
            float _FeatherAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv); // Original scene color
                float fogValue = tex2D(_FogTexture, i.uv).r; // Fog texture value (red channel)

                // Feathering effect
                fogValue = saturate(fogValue * (1.0 + _FeatherAmount) - _FeatherAmount);

                // Desaturate when unseen
                float desaturationFactor = lerp(1.0, _DesaturationAmount, 1.0 - fogValue);
                float grayscale = dot(color.rgb, float3(0.3, 0.59, 0.11));
                color.rgb = lerp(float3(grayscale, grayscale, grayscale), color.rgb, desaturationFactor);

                // Apply transparency
                color.a = fogValue;

                return color;
            }
            ENDCG
        }
    }
}