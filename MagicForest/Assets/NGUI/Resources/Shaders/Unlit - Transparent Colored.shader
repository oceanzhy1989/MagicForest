Shader "Unlit/Transparent Colored"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "black" {}
		_AlphaTex("Alpha (A)", 2D) = "black" {}
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
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
#pragma shader_feature USE_ALPHA_TEX	
#pragma multi_compile __ CLIP_1 CLIP_2 CLIP_3
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			//float4 _MainTex_ST;
#ifdef USE_ALPHA_TEX
			sampler2D _AlphaTex;
			//float4 _AlphaTex_ST;
#endif
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
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
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
	
			v2f o;

			v2f vert (appdata_t v)
			{
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
				
			fixed4 frag (v2f i) : COLOR
			{
				//fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				//return col;
				fixed4 col = tex2D(_MainTex, i.texcoord);
#ifdef USE_ALPHA_TEX
				col.a = tex2D(_AlphaTex, i.texcoord).r;
#endif
				float bgray = i.color.r <= 0.01;
				float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
				col = lerp(col * i.color, float4(gray, gray, gray, col.a), bgray);

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
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
