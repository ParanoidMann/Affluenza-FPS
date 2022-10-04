#ifndef STYLIZED_CGINC
#define STYLIZED_CGINC

#include "UnityCG.cginc"
#include "Noise.cginc"

sampler2D _MainTex;
float4 _MainTex_TexelSize;
float _Saturation, _Brightness, _Contrast;

// brightness, contrast, saturation modify
float3 ContrastSaturationBrightness (float3 color, float brt, float sat, float con)
{
	float3 brtColor = color * brt;
	float lumn = dot(brtColor, float3(0.2125, 0.7154, 0.0721));  // brigntess calculations
	float3 satColor = lerp(lumn.xxx, brtColor, sat);  // saturation calculation
	return lerp(0.5, satColor, con);  // contrast calculations
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

half4 _Color;
float _Angle, _Scale;

float pattern (float2 uv)
{
	float s = sin(_Angle);
	float c = cos(_Angle);
	half2 tex = uv * _ScreenParams.xy - 0.5;
	half2 pt = half2(c * tex.x - s * tex.y, s * tex.x + c * tex.y) * _Scale;
	return (sin(pt.x) * cos(pt.y)) * 2.0;
}
float4 fragDot (v2f_img input) : SV_Target
{
	half2 uv = input.uv;

	float4 mc = tex2D(_MainTex, uv);
	float average = (mc.r + mc.g + mc.b) / 3.0;

	float dst = 0.0;
	if (average < 0.001)
		dst = 0.0;
	else if (average > 0.45)
		dst = 1.0;
	else
		dst = average * 10.0 - 2.0 + pattern(uv);

	if (dst < 1.0)
		dst = 0.2;

	dst = clamp(dst, 0.2, 1.0);
	return float4(_Color.rgb * dst, dst);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

sampler2D _CameraDepthNormalsTexture;
half4 _Sensitivity, _LineColor;
half _SampleDistance, _LineNoise, _LineIntensity;

inline half CheckSame (half2 centerNormal, float centerDepth, half4 theSample)
{
	half2 diff = abs(centerNormal - theSample.xy) * _Sensitivity.y;
	half isSameNormal = (diff.x + diff.y) * _Sensitivity.y < 0.1;

	float sampleDepth = DecodeFloatRG(theSample.zw);
	float zdiff = abs(centerDepth - sampleDepth);

	half isSameDepth = zdiff * _Sensitivity.x < 0.09 * centerDepth;
	return isSameNormal * isSameDepth;
}
half4 fragOutline (v2f_img input) : SV_Target
{
	half2 uv = input.uv;

	half sampleDist = _SampleDistance * 2.4;
	half4 colSample1 = tex2D(_MainTex, uv + float2(0, -_MainTex_TexelSize.y) * sampleDist);
	half4 colSample2 = tex2D(_MainTex, uv + float2(0, +_MainTex_TexelSize.y) * sampleDist);
	half4 colSample3 = tex2D(_MainTex, uv + float2(-_MainTex_TexelSize.x, 0) * sampleDist);
	half4 colSample4 = tex2D(_MainTex, uv + float2(+_MainTex_TexelSize.x, 0) * sampleDist);

	half4 center = tex2D(_CameraDepthNormalsTexture, uv);//return center;
	half4 sample1 = tex2D(_CameraDepthNormalsTexture, uv + float2(0, -_MainTex_TexelSize.y) * sampleDist);
	half4 sample2 = tex2D(_CameraDepthNormalsTexture, uv + float2(0, +_MainTex_TexelSize.y) * sampleDist);
	half4 sample3 = tex2D(_CameraDepthNormalsTexture, uv + float2(-_MainTex_TexelSize.x, 0) * sampleDist);
	half4 sample4 = tex2D(_CameraDepthNormalsTexture, uv + float2(+_MainTex_TexelSize.x, 0) * sampleDist);

	half2 nrm = center.xy;   // center normal
	float dpt = DecodeFloatRG(center.zw);   // center depth

	half edge = (CheckSame(nrm, dpt, sample1) + CheckSame(nrm, dpt, sample2) + CheckSame(nrm, dpt, sample3) + CheckSame(nrm, dpt, sample4)) / 4.0;

	half v1 = (colSample1.r + colSample1.g + colSample1.b) / 3.0;
	half v2 = (colSample2.r + colSample2.g + colSample2.b) / 3.0;
	half v3 = (colSample3.r + colSample3.g + colSample3.b) / 3.0;
	half v4 = (colSample4.r + colSample4.g + colSample4.b) / 3.0;
	half avg = (v1 + v2 + v3 + v4) / 4.0;

	half4 c = _LineColor;
	c.a = 1.0 - edge;

	half2 pos = uv * _ScreenParams;
	float nz = snoise(half2(pos.x * pos.y, pos.x + pos.y));
	nz = lerp(1.0, nz, _LineNoise);
	c.a *= 1.0 - dpt * 2.0;
	c.a *= abs(nz) * _LineIntensity;
//return c.aaaa;
	return c;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

half _Step, _StepNoiseScale;

float stepColor (float c) { return floor(saturate(c) * (_Step * 2) / 2) / (_Step * 2) * 2; }
float4 fragNoiseDraw (v2f_img input) : SV_Target
{
	half2 uv = input.uv;
	half2 pos = uv * _ScreenParams;

	float4 c = tex2D(_MainTex, uv);
	float average = (c.r + c.g + c.b) / 3.0;
	float nz = snoise(half2(pos.x * pos.y + 1.0, pos.x + pos.y - 1.0));

	float depth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, uv).zw);
	if (depth < 0.99)
		average *= depth * 4.0 + 0.5;

	float sc = stepColor(average + (nz * _StepNoiseScale)) + 0.2;
	return float4(sc.xxx, c.a);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

sampler2D _OverlayTex, _PaperTex;
half _Intensity;

half4 fragOverlay (v2f_img input) : SV_Target
{
	half2 uv = input.uv;
	half4 o = tex2D(_OverlayTex, uv);
	half4 c = tex2D(_MainTex, uv);

	float3 check = step(0.5, c.rgb);

	float3 result = check * (1.0 - ((1.0 - 2.0 * (c.rgb - 0.5)) * (1.0 - o.rgb)));
	result += (1.0 - check) * (2.0 * c.rgb) * o.rgb;
	return half4(lerp(c.rgb, result.rgb, _Intensity), c.a);
}
half4 fragAlphaBlend (v2f_img input) : SV_Target
{
	half2 uv = input.uv;
	half4 c1 = tex2D(_OverlayTex, uv);
	half4 c2 = tex2D(_MainTex, uv);
	half4 c3 = tex2D(_PaperTex, uv);
	return lerp(c2, c1, c1.a) * c3;
}

#endif