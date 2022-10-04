Shader "Retro Game Effects/CRT/Function" {
	Properties {
		_MainTex ("Main", 2D) = "white" {}
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
	ENDCG
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float4 _BlurKernel;
			half _BlurZ;

			half4 blur (half2 uv)
			{
				float cornerWeight = _BlurKernel.x * _BlurKernel.x;
				float edgeWeight   = _BlurKernel.x * _BlurKernel.y;
				float centerWeight = _BlurKernel.y * _BlurKernel.y;

				float dtw = _MainTex_TexelSize.x;
				float dth = _MainTex_TexelSize.y;

				half3 c = 0.0;
				c += tex2D(_MainTex, uv + float2(-dtw, -dth)).rgb * cornerWeight;
				c += tex2D(_MainTex, uv + float2(0,    -dth)).rgb * edgeWeight;
				c += tex2D(_MainTex, uv + float2(dtw,  -dth)).rgb * cornerWeight;

				c += tex2D(_MainTex, uv + float2(-dtw, 0)).rgb * edgeWeight;
				c += tex2D(_MainTex, uv + float2(0,    0)).rgb * centerWeight;
				c += tex2D(_MainTex, uv + float2(dtw,  0)).rgb * edgeWeight;

				c += tex2D(_MainTex, uv + float2(-dtw, dth)).rgb * cornerWeight;
				c += tex2D(_MainTex, uv + float2(0,    dth)).rgb * edgeWeight;
				c += tex2D(_MainTex, uv + float2(dtw,  dth)).rgb * cornerWeight;
				return half4(c / _BlurZ, 1.0);
			}
			half4 frag (v2f_img input) : SV_Target
			{
				return blur(input.uv);
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			half4 _BgColor;

			float mod (float x, float y) { return x - y * floor(x / y); }
			half4 frag (v2f_img input) : SV_Target
			{
				float BUFFER = 2.0;
				float SPEED = 1.5;

				float t = mod(_Time.y * 4.0, 1.0 / SPEED + 1.0 + BUFFER * 2.0);
				t = clamp(t - BUFFER, 0.0, 1.0 / SPEED + 1.0);

				float pixelScale = (_MainTex_TexelSize.w - 1.0) / _MainTex_TexelSize.w;
				float scaleTime = clamp(t * SPEED, 0.0, pixelScale);
				float fadeTime = saturate(t - pixelScale / SPEED);

				float2 uv = input.uv;
				float2 scaledUV = float2((uv.x - 0.5) * (1.0 - scaleTime) + 0.5, (uv.y - 0.5) / (1.0 - scaleTime) + 0.5);

				float4 tc = tex2D(_MainTex, scaledUV) + float4(scaleTime.xxx, 0.0);
				float cropPixel = min(saturate(sign(abs(scaleTime / 2.0 - 0.5) - abs(uv.y - 0.5))),
					saturate(sign(1.0 - fadeTime - abs(uv.x - 0.5))));

				return lerp(_BgColor, lerp(_BgColor, tc, 1.0 - fadeTime), cropPixel);
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			half _LineAmount, _Speed;

			float4 applyScanlines (float4 color, float2 coord, float number, float amount, float power, float speed)
			{
				coord.y += _Time.y * speed;
				float darkenAmount = 0.5 + 0.5 * cos(coord.y * 6.28 * number);
				darkenAmount = pow(darkenAmount, power);

				color.rgb -= darkenAmount * amount;
				return color;
			}
			float2 bulgeUvCoords (float2 coord, float amount)
			{
				float dist = distance(coord, 0.5) * 2.0;
				float2 uv = coord;
				uv.xy -= 0.5;
				uv.xy *= 1.0 + dist * amount;
				uv.xy *= 1.0 - amount;
				uv.xy += 0.5;
				return uv;
			}
			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = input.uv;
				float2 newUv = bulgeUvCoords(uv, 0.2);
				float4 tc = tex2D(_MainTex, uv);
				return applyScanlines(tc, newUv, 150.0, _LineAmount, 2.0, _Speed);
			}
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _VignetteStr, _VignetteSize;
			float _CrtBendX, _CrtBendY, _CrtOverscan;

			half4 alphaBlend (half4 top, half4 bottom)
			{
				half4 c = 0.0;
				c.a   = top.a + bottom.a * (1 - top.a);
				c.rgb = (top.rgb * top.aaa + bottom.rgb * bottom.aaa * (1 - top.aaa)) / c.aaa;
				return c;
			}
			half3 vignette (float2 uv)
			{
				float dist = distance(0.5, uv) * 1.414213;   // multiplyed by 1.414213 to fit in the range of 0 to 1
				return saturate((1.0 - dist) / (1.0 - _VignetteSize)).xxx;
			}
			float2 crt (float2 coord, float bendX, float bendY)
			{
				coord = (coord - 0.5) * 2 / (_CrtOverscan + 1);  // to symmetrical coords

				// bend
				coord.x *= 1 + pow((abs(coord.y) / bendX), 3);
				coord.y *= 1 + pow((abs(coord.x) / bendY), 3);

				coord = (coord / 2) + 0.5;  // transform back to 0~1
				return coord;
			}
			half4 frag (v2f_img input) : SV_Target
			{
				float2 uv = crt(input.uv, _CrtBendX, _CrtBendY);
				if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1)
				{
					return half4(0, 0, 0, 1);
				}
				else
				{
					half4 final = tex2D(_MainTex, uv);
					half3 tmp = final.rgb * vignette(uv);
					final.rgb = alphaBlend(half4(tmp, _VignetteStr), final).rgb;
					return final;
				}
			}
			ENDCG
		}
	}
	Fallback Off
}