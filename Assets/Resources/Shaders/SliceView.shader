shader "DICOM/SliceView"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VolumeTex("3D Volume Texture", 3D) = "" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler3D _VolumeTex;

            uniform float4x4 _LocalToWorld;
            uniform float4x4 _WorldToVolume;

            float4 _MainTex_ST;
      
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uvw : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvw = mul(_WorldToVolume, mul(_LocalToWorld, v.vertex));
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                if(i.uvw.y > 0.5)
                    return float4(0.0f, 0.0f, 0.0f, 1.0);

                float value = tex3D(_VolumeTex, i.uvw);
                return float4(value, value, value, 1.0f);
            }

            ENDCG
        }
    }
}