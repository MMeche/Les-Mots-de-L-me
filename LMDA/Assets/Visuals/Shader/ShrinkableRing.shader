Shader "UI/ShrinkableRing_SeparateAlpha"
{
    Properties
    {
        _Thickness ("Thickness (px)", Float) = 4
        _Radius ("Radius", Range(0,1)) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ UNITY_UI_CLIP_RECT

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            float _Thickness;
            float _Radius;
            float4 _ClipRect;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = v.vertex;

                // UV centrées
                o.uv = v.uv * 2.0 - 1.0;

                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                #ifdef UNITY_UI_CLIP_RECT
                if (UnityGet2DClipping(i.worldPosition.xy, _ClipRect) < 0)
                    discard;
                #endif

                float r = length(i.uv);

                // Conversion pixel (épaisseur constante)
                float pixelSize = fwidth(r);

                float width = _Thickness * pixelSize;

                // distance uniquement vers l'intérieur
                float innerDist = _Radius - r;

                // anneau uniquement à l'intérieur
                float ring = smoothstep(0.0, pixelSize, innerDist) *
                             (1.0 - smoothstep(width, width + pixelSize, innerDist));

                fixed4 col = i.color;

                //
                col.a *= ring;

                return col;
            }
            ENDCG
        }
    }
}