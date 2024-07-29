shader "DICOM/Slice"
{
    Properties
    {
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

            sampler3D _VolumeTex;
            uniform float4x4 _WorldToVolume;
            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uvw : TEXCOORD0;
            };

            bool OutOfBounds(float3 uvw)
            {
                float eps = 0.00001f;
                float upper = 1.0f + eps;
                float lower = 0.0f - eps;
                return(uvw.x > upper || uvw.y > upper || uvw.z > upper || uvw.x < lower || uvw.y < lower || uvw.z < lower);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvw = mul(_WorldToVolume, mul(unity_ObjectToWorld, v.vertex));
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 dataCoord = i.uvw + float3(0.5f, 0.5f, 0.5f);

                if(OutOfBounds(dataCoord))
                    return float4(0.0f, 0.0f, 0.0f, 1.0f);

                float value = tex3D(_VolumeTex, dataCoord);
                return float4(value, value, value, 1.0f);
            }

            ENDCG
        }
    }
}