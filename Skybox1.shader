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

                RefColor = RefColor * 255.0;
                float l = max(max(RefColor.r, RefColor.g), RefColor.b);
                float Tr;
                float Tg;
                float Tb;
                float ratio = _UVector.w / viewD.w;
                if(l > 1)
                {
                    RefColor = RefColor * 255.0 / l;
                    Tb = 1000.0 + 904.495 * exp(0.00721929 * RefColor.b);
                    Tr = 6000.0 + 8.01879e20 * pow(max(RefColor.r, 1.0), -7.507239275877164);
                    if(RefColor.b > 254.0)
                    {
                        Tg = 6502.86;
                    } else {
                        if(RefColor.b / max(RefColor.r, 1.0) > 0.98084)
                        {
                            Tg = 505.192 * exp(0.0100532 * RefColor.g);
                        } else {
                            Tg = 6000.0 + 3.55446e34 * pow(max(RefColor.g, 1.0), -13.24242861627803);
                        }
                    }

                    Tr = Tr * ratio;
                    Tg = Tg * ratio;
                    Tb = Tb * ratio;

                    if(6689.0 > Tr)
                    {
                        RefColor.r = 255.0;
                    } else {
                        RefColor.r = 608.873 * pow(Tr - 6000.0, -0.133205);
                    }
                    if(506.0 > Tg)
                    {
                        RefColor.g = 0.0;
                    } else if(6502.86 > Tg){
                        RefColor.g = -619.2 + 99.4708 * log(Tg);
                    } else {
                        RefColor.g = 406.534 * pow(Tg - 6000.0, -0.0755148);
                    }
                    if(1905.0 > Tb)
                    {
                        RefColor.b = 0.0;
                    } else if(6700.0 < Tb){
                        RefColor.b = 255.0;
                    } else {
                        RefColor.b = -305.045 + 138.518 * log(0.01 * Tb - 10.0);
                    }
                    RefColor = RefColor * l / pow(255.0, 2.0);
                } else {
                    RefColor = RefColor * 0.0;
                }
                return float4(RefColor.r, RefColor.g, RefColor.b, 1);
            }
            ENDCG
        }
    }
}

