Shader "Unlit/WireframeShader"
{
    Properties
    {
        _WireframeWidth("Wireframe width threshold", Float) = 0.025
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        Blend One Zero 

        Pass
        {
            Cull Off 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float3 barycentric : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream)
            {
                g2f o;
                o.pos = IN[0].vertex; o.barycentric = float3(1,0,0); triStream.Append(o);
                o.pos = IN[1].vertex; o.barycentric = float3(0,1,0); triStream.Append(o);
                o.pos = IN[2].vertex; o.barycentric = float3(0,0,1); triStream.Append(o);
            }

            float _WireframeWidth;

            fixed4 frag(g2f i) : SV_Target
            {
                float edgeFactor = min(i.barycentric.x, min(i.barycentric.y, i.barycentric.z));
                float isEdge = step(edgeFactor, _WireframeWidth);

                // Faces = black (0,0,0), Edges = white (1,1,1)
                return lerp(fixed4(0,0,0,1), fixed4(1,1,1,1), isEdge);
            }
            ENDCG
        }
    }
}
