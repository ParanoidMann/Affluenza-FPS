Shader "Retro Game Effects/OldFilm" {
	Properties {
		[HideInInspector]_MainTex ("", 2D) = "white" {}
		[Header(Effect1)][Space(5)]
		_Fade1        ("Fade", Range(0, 1)) = 0.7
		_Bright       ("Brightness", Float) = 16
		_Noise        ("Noise Intensity", Range(0, 1.6)) = 0.8
		_PixelDensity ("Pixel Density", Float) = 250

		[Header(Effect2)][Space(5)]
		_PixelX ("Pixel X", Float) = 600
		_PixelY ("Pixel Y", Float) = 600

		[Header(Effect3)][Space(5)]
		_Fade3       ("Fade", Range(0, 1)) = 1
		_Speed3      ("Speed", Float) = 13
		_Exposure    ("Exposure", Range(0, 2)) = 1
		_ColorOffset ("Color Offset", Range(-1, 1)) = 0.08

		[Header(Effect4)][Space(5)]
		_Grain    ("Grain", Range(0, 1)) = 0.5
		_Speed4   ("Speed", Float) = 13
		_Vignette ("Vignette", Float) = 62
		_BarWidth ("Bar Width", Range(1, 256)) = 200
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		float rand (float2 v) { return frac(sin(dot(v, float2(12.9898, 78.233))) * 43758.5453); }
		float rand (float f)  { return rand(float2(f, 1.0)); }
		float4 _Offset;
	ENDCG
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _MainTex;
			half _Fade1, _Noise, _PixelDensity, _Bright;

			float2 curve (float2 uv)
			{
				uv    = (uv - 0.5) * 2.0;
				uv   *= 1.1;
				uv.x *= 1.0 + pow((abs(uv.y) * 0.2), 2.0);
				uv.y *= 1.0 + pow((abs(uv.x) * 0.25), 2.0);
				uv    = (uv / 2.0) + 0.5;
				uv    =  uv *0.92 + 0.04;
				return uv;
			}
			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = saturate(curve(input.uv + _Offset));
				float x = sin(0.3 * _Time.y + uv.y * 21.0) * sin(0.7 * _Time.y + uv.y * 29.0) * sin(0.3 + 0.33 * _Time.y + uv.y * 31.0) * 0.0017;
				float2 newUv = lerp(uv, float2(x + uv.x + 0.001, uv.y + 0.001), _Fade1);
				half4 tc = tex2D(_MainTex, newUv);
				half4 orig = tc;
				half3 col = tc.rgb + 0.05;
				tc = tex2D(_MainTex, 0.75 * float2(x + 0.025, -0.02) + float2(uv.x + 0.001, uv.y + 0.001));
				col += half3(0.08 * tc.r, 0.05 * tc.g, 0.08 * tc.b);
				col = saturate(col * 0.6 + 0.4 * col * col);
				float vig = _Bright * uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
				col *= pow(vig, 0.3);
				col *= half3(3.66, 2.94, 2.66);
				float scans = saturate(0.35 + 0.35 * sin(3.5 * _Time.y + uv.y * _ScreenParams.y * 1.5));
				scans = pow(scans, 1.7);
				col = col * (0.4 + 0.7 * scans);
				col.rgb = dot(col.rgb, half3(0.222, 0.707, 0.071));
				float cx = saturate(rand(float2(floor(uv.x * _PixelDensity), floor(uv.y * _PixelDensity)) * _Time.y / 1000.0) + 1.0 - _Noise);

				float bar = clamp(floor(sin(uv.y * 6.0 + _Time.y * 2.6) + 1.95), 0, 1.1);
				col *= 1.0 + 0.01 * sin(110.0 * _Time.y) + cx.xxx * bar;
				col = lerp(orig, col, _Fade1);
				return half4(col, 1.0);
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _MainTex;
			half _PixelX, _PixelY;

			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = input.uv + _Offset;
				float3 orig = tex2D(_MainTex, uv).rgb;
				float bar = saturate(sin(uv.y * 6.0 + _Time.y * 5.6) + 1.25);
				float3 c = saturate(rand(float2(floor(uv.x * _PixelX), floor(uv.y * _PixelY)) * _Time.y / 1000.0) + 0.5);
				c = lerp(c - 3.0 * 0.25, c, bar);
				c = (lerp(0.0, orig, c)).x;
				c.b += 0.052;
				c *= 1.0 - pow(distance(uv, 0.5), 2.1) * 2.8;
				return half4(c, 1.0);
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _MainTex;
			float _Exposure, _ColorOffset, _Fade3, _Speed3;

			float randomLine (float seed, float2 uv)
			{
				float a = rand(seed + 1.0);
				float b = 0.01 * a;
				float c = a - 0.5;
				float l = 0.0;
				if (a > 0.2)
					l = pow(abs(a * uv.x + b * uv.y + c), 0.125);
				else
					l = 2.0 - pow(abs(a * uv.x + b * uv.y + c), 0.125);
				return lerp(-0.5, 1.0, l);
			}
			float randomBlotch (float seed, float2 uv)
			{
				float x = rand(seed);
				float y = rand(seed + 1.0);
				float s = 0.01 * rand(seed + 2.0);
				float2 p = float2(x, y) - uv;
				float aa = atan(p.y / p.x);
				float v = 0.0;
				float ss = s * s * (sin(6.2831 * aa * x) * 0.1 + 1.0);
				if (dot(p, p) < ss)
					v = 0.2;
				else
					v = pow(dot(p, p) - ss, 1.0 / 16.0);
				return lerp(0.3 + 0.2 * (1.0 - (s / 0.02)) - 1.0, 1.0, v);
			}
			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = input.uv + _Offset;
				float t = float(int(_Time.y * _Speed3));
				float2 suv = uv + 0.002 * float2(rand(t), rand(t + 23.0));
				float luma = dot(float3(0.2126, 0.7152, 0.0722), tex2D(_MainTex, suv).rgb);
				float3 image = luma * float3(0.7 + _ColorOffset, 0.7 + _ColorOffset / 2, 0.7) * _Exposure;
				image *= float3(0.7 + _ColorOffset, 0.7 + _ColorOffset / 8, 0.7) * _Exposure;
				float randx = rand(t + 8.0);
				float vI = 16.0 * (uv.x * (1.0-uv.x) * uv.y * (1.0-uv.y));
				vI *= lerp(0.7, 1.0, randx + 0.5);
				vI += 1.0 + 0.4 * randx;

				// vignetting
				vI *= pow(16.0 * uv.x * (1.0 - uv.x) * uv.y * (1.0 - uv.y), 0.4);

				// random lines
				int l = int(8.0 * randx);
				if (0 < l) vI *= randomLine(t + 6.0, uv);
				if (1 < l) vI *= randomLine(t + 23.0, uv);

				// random blotches
				int s = int(max(8.0 * rand(t + 18.0) - 2.0, 0.0));
				if (0 < s) vI *= randomBlotch(t + 6.0, uv);
				if (1 < s) vI *= randomBlotch(t + 25.0, uv);
				float4 c = float4(image * vI, 1.0);

				// grain
				c *= (1.0 + (rand(uv + t * 0.01) - 0.2) * 0.15);

				c = lerp(c, tex2D(_MainTex, uv), 1.0 - _Fade3);
				return c;
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _MainTex;
			float _Grain, _Vignette, _Speed4, _BarWidth;

			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = input.uv + _Offset.xy;

				half3 rgb = tex2D(_MainTex, uv).rgb;
				float gray = dot(rgb, float3(0.2126, 0.7152, 0.072));

				float t = float(int(_Time.y * _Speed4));
				float rnd = ((rand(uv + t * 0.01) - 0.2) * 0.15);
				float grain = (1.0 - _Grain) * gray + rnd * _Grain;
				grain *= 2,0;

				float vign = saturate(pow(uv.x, _Vignette)) + saturate(pow(1.0 - uv.x, _Vignette)) + saturate(pow(uv.y, _Vignette)) + saturate(pow(1.0 - uv.y, _Vignette));
				float bar = saturate(_BarWidth * abs(uv.x - rand(_Time.y)));
				return half4((grain * bar - vign).xxx, 1.0);
			}
			ENDCG
		}
	}
	Fallback Off
}