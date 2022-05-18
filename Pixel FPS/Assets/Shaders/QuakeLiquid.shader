Shader "Unlit/QuakeLiquid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveFrequency ("Wave Frequency", Float) = 0.5
        _WaveScale ("Wave Scale", Float) = 0.8
        _WaveAmplitude ("Wave Amplitude", Float) = 0.15
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
 
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WaveScale, _WaveAmplitude, _WaveFrequency;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                float2 sinTime = (i.uv.yx * _WaveScale + _Time.y * _WaveFrequency) * UNITY_PI;
                float2 uv = i.uv + float2(sin(sinTime.x), sin(sinTime.y)) * _WaveAmplitude;
                fixed4 col = tex2D(_MainTex, uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}