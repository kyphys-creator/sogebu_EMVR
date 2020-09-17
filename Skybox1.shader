Shader "Unlit/Skybox1"
{
    Properties {
        // キューブマップテクスチャのプロパティ
        _Cube ("Cube", CUBE) = "" {}
        _UVector ("UVector", Vector) = (0,0,0,0) 
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform float4 _UVector;
            struct appdata {
                float4 vertex: POSITION;
                half3 normal: NORMAL;
                half2 uv: TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                float3 pos2 : TEXCOORD1;
                half3 normal : TEXCOORD2;
            };

            /*UNITY_SAMPLE_TEXCUBEで使用する変数を定義する*/
              UNITY_DECLARE_TEXCUBE(_Cube);
              float4x4 L;
              float4x4 R;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.pos2 = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                float3 viewDir = normalize(i.pos2 - _WorldSpaceCameraPos);
                _UVector.xyz = viewDir;
                _UVector.w = -1;//-sqrt(_UVector.x*_UVector.x + _UVector.y*_UVector.y + _UVector.z*_UVector.z)
                float4 viewD = mul(L,_UVector);
                viewDir = viewD.xyz;
                /*キューブマップと反射方向のベクトルから反射先の色を取得する*/
                float4 RefColor = UNITY_SAMPLE_TEXCUBE(_Cube, viewDir);

                
                return float4(RefColor.r, RefColor.g, RefColor.b, 1);
            }
            ENDCG
        }
    }
}

