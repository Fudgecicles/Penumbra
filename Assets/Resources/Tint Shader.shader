Shader "Tint Shader" {
    Properties {
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader {
        Pass {
        	Blend SrcColor DstAlpha
        	CGPROGRAM
        	#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			uniform float4 _Color;
			
			struct vertexInput
			{
			    float4 vertex: POSITION;
			    float4 color : COLOR;   
			};
			struct fragmentInput
			{
			    float4 pos : SV_POSITION;
			    float4 color : COLOR0; 
			};
			fragmentInput vert( vertexInput i )
			{
			    fragmentInput o;
			    o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
			    o.color = _Color;
			    return o;
			}
			half4 frag(fragmentInput i) : COLOR {
				return i.color;
			}
        	ENDCG
        }
    } 
}