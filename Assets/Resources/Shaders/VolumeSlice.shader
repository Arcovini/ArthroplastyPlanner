shader "VolumeSlice"
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
            
            uniform float3 AABBmin;
            uniform float3 AABBmax;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 volumeVertex : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            bool Outside(float3 uvw)
            {
                return(uvw.x > AABBmax.x || uvw.y > AABBmax.y || uvw.z > AABBmax.z || uvw.x < AABBmin.x || uvw.y < AABBmin.y || uvw.z < AABBmin.z);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 vert = mul(unity_ObjectToWorld, v.vertex);
                o.volumeVertex = mul(_WorldToVolume, vert);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 dataCoord = i.volumeVertex;

                if(Outside(dataCoord))
                {
                    return float4(0.0f, 0.0f, 0.0f, 1.0f);
                }

                float uvOffset = 0.5f * (AABBmax - AABBmin);
                float dataVal = tex3D(_VolumeTex, dataCoord + uvOffset);
                return float4(dataVal, dataVal, dataVal, 1.0f);
            }

            ENDCG
        }
    }
}