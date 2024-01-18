Shader "Custom/DiagonalLinesUI" {
    Properties {
        _MainTex ("Texture", 2D) = "white" { }
        _Color ("Line Color", Color) = (1,1,1,1)
        _LineSize ("Line Size", Range(0.001, 1)) = 0.01
        _LineSpacing ("Line Spacing", Range(0.001, 1)) = 0.02
        _Speed ("Animation Speed", Range(0.1, 10)) = 1

        _MainTex ("Base (RGB)", 2D) = "white" { }
    }

    SubShader {
        Tags { "Queue" = "Overlay" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _LineSize;
            float _LineSpacing;
            float _Speed;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : COLOR {
                // Create animated diagonal lines pattern
                float diagonalLines = fmod((i.uv.x + i.uv.y + _Time.y * _Speed), _LineSpacing) < _LineSize ? 1 : 0;

                // Apply color
                fixed4 col = _Color * tex2D(_MainTex, i.uv);
                col.a *= diagonalLines;

                return col;
            }
            ENDCG
        }
    }
}
