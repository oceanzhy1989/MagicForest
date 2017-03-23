Shader "Unlit/Text"
{
	Properties
	{
		_MainTex ("Alpha (A)", 2D) = "white" {}
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Offset -1, -1
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
#pragma multi_compile __ CLIP_1 CLIP_2 CLIP_3
				#include "UnityCG.cginc"

#ifdef CLIP_1
				float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
				float4 _ClipArgs0 = float4(1000.0, 1000.0, 0.0, 1.0);
#elif CLIP_2
				float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
				float4 _ClipArgs0 = float4(1000.0, 1000.0, 0.0, 1.0);
				float4 _ClipRange1 = float4(0.0, 0.0, 1.0, 1.0);
				float4 _ClipArgs1 = float4(1000.0, 1000.0, 0.0, 1.0);
#elif CLIP_3
				float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
				float4 _ClipArgs0 = float4(1000.0, 1000.0, 0.0, 1.0);
				float4 _ClipRange1 = float4(0.0, 0.0, 1.0, 1.0);
				float4 _ClipArgs1 = float4(1000.0, 1000.0, 0.0, 1.0);
				float4 _ClipRange2 = float4(0.0, 0.0, 1.0, 1.0);
				float4 _ClipArgs2 = float4(1000.0, 1000.0, 0.0, 1.0);
#endif
				struct appdata_t
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord : TEXCOORD0;
#ifdef CLIP_1
					float4 worldPos : TEXCOORD1;
#elif CLIP_2
					float4 worldPos : TEXCOORD1;
#elif CLIP_3
					float4 worldPos : TEXCOORD1;
					float2 worldPos2 : TEXCOORD2;
#endif
				};

				float2 Rotate(float2 v, float2 rot)
				{
					float2 ret;
					ret.x = v.x * rot.y - v.y * rot.x;
					ret.y = v.x * rot.x + v.y * rot.y;
					return ret;
				}

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = v.texcoord;
					o.color = v.color;
#ifdef CLIP_1
					o.worldPos.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
#elif CLIP_2
					o.worldPos.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
					o.worldPos.zw = Rotate(v.vertex.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
#elif CLIP_3
					o.worldPos.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
					o.worldPos.zw = Rotate(v.vertex.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
					o.worldPos2 = Rotate(v.vertex.xy, _ClipArgs2.zw) * _ClipRange2.zw + _ClipRange2.xy;
#endif
					return o;
				}

				half4 frag (v2f i) : COLOR
				{
					half4 col = i.color;
					col.a *= tex2D(_MainTex, i.texcoord).a;

#ifdef CLIP_1
					float2 factor = (float2(1.0, 1.0) - abs(i.worldPos.xy)) * _ClipArgs0.xy;

					col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
#elif CLIP_2
					// First clip region
					float2 factor = (float2(1.0, 1.0) - abs(i.worldPos.xy)) * _ClipArgs0.xy;
					float f = min(factor.x, factor.y);

					// Second clip region
					factor = (float2(1.0, 1.0) - abs(i.worldPos.zw)) * _ClipArgs1.xy;
					f = min(f, min(factor.x, factor.y));

					col.a *= clamp(f, 0.0, 1.0);
#elif CLIP_3
					// First clip region
					float2 factor = (float2(1.0, 1.0) - abs(i.worldPos.xy)) * _ClipArgs0.xy;
					float f = min(factor.x, factor.y);

					// Second clip region
					factor = (float2(1.0, 1.0) - abs(i.worldPos.zw)) * _ClipArgs1.xy;
					f = min(f, min(factor.x, factor.y));

					// Third clip region
					factor = (float2(1.0, 1.0) - abs(i.worldPos2)) * _ClipArgs2.xy;
					f = min(f, min(factor.x, factor.y));

					col.a *= clamp(f, 0.0, 1.0);
#endif
					return col;
				}
			ENDCG
		}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		
		Lighting Off
		Cull Off
		ZTest Always
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		
		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		Pass
		{
			SetTexture [_MainTex]
			{ 
				combine primary, texture
			}
		}
	}
}
