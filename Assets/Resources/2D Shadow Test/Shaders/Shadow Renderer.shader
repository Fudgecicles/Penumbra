Shader "Custom/Shadow/Distance" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}

	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f {
		float4 pos : POSITION;
		float2 uv[3] : TEXCOORD0;
	};

	sampler2D _SrcTex;
	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
		
	//external
	//float _Width;
	
	float _MinLuminance;
	float _ShadowOffset;
	
	//My variables
	float _Range;
	float _Intensity;

	v2f vert(appdata_img v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.uv[0] = v.texcoord.xy;
		o.uv[1] = v.texcoord.xy;

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv[0].y = 1-o.uv[0].y;
		#endif

		return o;
	}

 	v2f vertOffset(appdata_img v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.uv[0] = v.texcoord.xy;
		o.uv[1] = v.texcoord.xy;

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv[0].y = 1-o.uv[0].y;
		#endif

		o.uv[2] = o.uv[1];
		o.uv[2].x -= _MainTex_TexelSize.x;

		return o;
	}

 	v2f vertQuadrant(appdata_img v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.uv[0] = v.texcoord.xy;
		o.uv[1] = v.texcoord.xy;

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv[0].y = 1-o.uv[0].y;
		#endif

		o.uv[2].x = 2.0f * (o.uv[1].x - 0.5f);
		o.uv[2].y = 2.0f * (o.uv[1].y - 0.5f);

		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed4 color = tex2D(_MainTex, i.uv[1]);
		// if the r color is darker han _MinLuminance it counts as a shadow
		//New Code
//		fixed distance = lerp(1.0f, length(i.uv[1] - 0.5), step(color.r, _MinLuminance));
//		if (length(i.uv[1] - 0.5f) > 0.15)
//		{
//			distance = 1;
//		}

		//Equivalent Code
		fixed distance = 1;
		if (color.r <= _MinLuminance) {
			distance = lerp(1.0f, length(i.uv[1] - 0.5), 1);
		}

		//Old Code
		//fixed distance = lerp(1.0f, length(i.uv[1] - 0.5), step(color.r, _MinLuminance));

		// save it to the Red channel
		return fixed4(distance, 0, 0, 1);
	}

	fixed4 fragStretch(v2f i) : COLOR
	{
		//translate u and v into [-1 , 1] domain
		float u0 = (i.uv[1].x) * 2 - 1;
		float v0 = (i.uv[1].y) * 2 - 1;

		//then, as u0 approaches 0 (the center), v should also approach 0
		v0 = v0 * abs(u0);
		//convert back from [-1,1] domain to [0,1] domain
		v0 = (v0 + 1) / 2;
		//we now have the coordinates for reading from the initial image
		float2 newCoords = float2(i.uv[1].x, v0);

		//read for both horizontal and vertical direction and store them in separate channels
		fixed horizontal = tex2D(_MainTex, newCoords).r;
		fixed vertical = tex2D(_MainTex, newCoords.yx).r;
		return fixed4(horizontal, vertical, 0, 1);
	}

	fixed4 fragSquish(v2f i) : COLOR
	{
		fixed2 color = tex2D(_MainTex, i.uv[1]);
		fixed2 colorR = tex2D(_MainTex, i.uv[2]);
		fixed2 result = min(color, colorR);
		return fixed4(result, 0, 1);
	}

	fixed4 fragShrink(v2f i) : COLOR
	{
		return tex2D(_MainTex, i.uv[1] * float2(2, 1));
	}

	fixed GetShadowDistanceH(float2 TexCoord)
	{
		float u = TexCoord.x;
		float v = TexCoord.y;

		u = abs(u - 0.5f) * 2;
		v = v * 2 - 1;
		float v0 = v/u;
		v0 = (v0 + 1) / 2;

		float2 newCoords = float2(TexCoord.x * _MainTex_TexelSize.x * 2, v0);

		//horizontal info was stored in the Red component
		return tex2D(_MainTex, newCoords).r;
	}

	fixed GetShadowDistanceV(float2 TexCoord)
	{
		float u = TexCoord.y;
		float v = TexCoord.x;

		u = abs(u - 0.5f) * 2;
		v = v * 2 - 1;
		float v0 = v/u;
		v0 = (v0 + 1) / 2;

		float2 newCoords = float2(TexCoord.y * _MainTex_TexelSize.x * 2, v0);

		//vertical info was stored in the Green component
		return tex2D(_MainTex, newCoords).g;
	}

	fixed4 fragShadow(v2f i) : COLOR
	{
		// 1.0f / _ShadowMapSize shift shadow by a pixel
		float distance = (length(i.uv[1] - 0.5f) - _ShadowOffset);

		//coords in [-1,1]
		//we use these to determine which quadrant we are in
		float nX = abs(i.uv[2].x);
		float nY = abs(i.uv[2].y);

		//if distance to this pixel is lower than distance from shadowMap, then we are not in shadow
		float shadowMapDistance =
			lerp(
				GetShadowDistanceH(i.uv[1]),
				GetShadowDistanceV(i.uv[1]),
			step(nX, nY));

		fixed light = step(distance, shadowMapDistance);
		
		//Macs Code
//		if(light * (0.02/ (distance * distance)) == 0)
//		{
//			return 1.0;
//		}
//		else
//		{
//			return 0.0;
//		}
//		if (shadowMapDistance < distance) {
//			_Intensity = 0.5;
//		}
		
		//New Code
		return (_Intensity / pow(10, _Range)) * (light) / pow (distance, _Range);

		//Old Code
		//return _Intensity * light * (_Range / (distance * distance));
	}

	// faked penumbra
	fixed4 fragShadowHQ(v2f i) : COLOR
	{
//		#define SAMPLE_COUNT 10
//		const float RAND_SAMPLES[SAMPLE_COUNT] = {
//			0.04,
//			-.04,
//			0.08,
//			-.08,
//			0.12,
//			-.12,
//			0.16,
//			-.16,
//			0.20,
//			-.20,
//		};	

		#define SAMPLE_COUNT 20
		const float RAND_SAMPLES[SAMPLE_COUNT] = {
			0.03,
			-.03,
			0.06,
			-.06,
			0.09,
			-.09,
			0.12,
			-.12,
			0.15,
			-.15,
			0.18,
			-.18,
			0.21,
			-.21,
			0.24,
			-.24,
			0.27,
			-.27,
			0.30,
			-.30,
		};
	
		// 1.0f / _ShadowMapSize shift shadow by a pixel
		float distance = length(i.uv[1] - 0.5f) - _ShadowOffset;

		float nX = abs(i.uv[2].x);
		float nY = abs(i.uv[2].y);

		float distanceSample =
				lerp(
					GetShadowDistanceH(i.uv[1]),
					GetShadowDistanceV(i.uv[1]),
				step(nX, nY));

		distanceSample = saturate(distance - distanceSample) * _MainTex_TexelSize.x;

		float shadow = 0;
		for(int k = 0; k < SAMPLE_COUNT; k++)
		{
			float shadowMapDistance =
				lerp(
					GetShadowDistanceH(i.uv[1] + RAND_SAMPLES[k].x * distanceSample),
					GetShadowDistanceV(i.uv[1] + RAND_SAMPLES[k].x * distanceSample),
				step(nX, nY));

			shadow += step(distance, shadowMapDistance);
		}

		//New code
		return (_Intensity / pow(10, _Range)) * (shadow / SAMPLE_COUNT) / pow (distance, _Range);
		
		//Old Code
		//return (shadow / SAMPLE_COUNT) * (0.02/ (distance * distance));
	}

	ENDCG

Subshader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		ENDCG
	}

	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment fragStretch
		ENDCG
	}

	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM
		#pragma vertex vertOffset
		#pragma fragment fragSquish
		ENDCG
	}

	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM
		#pragma vertex vertQuadrant
		#pragma fragment fragShadow
		ENDCG
	}

	// HQ
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM
		#pragma target 3.0
		#pragma vertex vertQuadrant
		#pragma fragment fragShadowHQ
		ENDCG
	}

	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment fragShrink
		ENDCG
	}
}

Fallback off

} // shader