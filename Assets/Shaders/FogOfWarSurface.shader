Shader "Custom/FogOfWarShader"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _FogMap ("Fog Map", 2D) = "white" {}
        _FogIntensity ("Fog Intensity", Range(0, 1)) = 0.8
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _BaseMap;
            sampler2D _FogMap;
            float _FogIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample base map and fog map
                fixed4 baseColor = tex2D(_BaseMap, i.uv);
                fixed4 fogColor = tex2D(_FogMap, i.uv);

                // Blend base color and fog color based on visibility
                fixed4 finalColor = lerp(baseColor, fogColor, _FogIntensity * fogColor.a);
                return finalColor;
            }
            ENDCG
        }
    }
}