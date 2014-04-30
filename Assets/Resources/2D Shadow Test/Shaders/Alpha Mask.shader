Shader "Alpha Mask" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Alpha ("Alpha (A)", 2D) = "white" {}
        _TintTex ("Tint (RGB)", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        Pass {
        	Blend SrcAlpha OneMinusSrcAlpha
        	SetTexture[_MainTex] {
                Combine texture
            }
            SetTexture[_Alpha] {
                combine previous, texture
                constantColor (0,0,0,1)
                combine previous lerp(texture) constant
            }
            SetTexture[_TintTex] {
            	constantColor [_TintColor]
            	combine previous +- constant
            }
            SetTexture[_Alpha] {
                combine previous, texture
            }
        }
    } 
}