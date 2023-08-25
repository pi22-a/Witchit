Shader "ImageEffect/RampEffectImage"

{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RampAmount("Ramp Amount", float) = 0
		_RampTex("Ramp Texture",2D) = "black"{}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			//어이없어 정의라니;
			sampler2D _MainTex;
			float _RampAmount;
			sampler2D _RampTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float luminance = col.r * 0.29 + col.g * 0.59 + col.b * 0.12;
				half4 Ramp = tex2D(_RampTex, float2(luminance, 0.5));
				half3 RampColor = Ramp.rgb;
				half BrightCurve = Ramp.a;
				col.rgb = lerp(col.rgb, RampColor * BrightCurve, _RampAmount);
				return col;
			}
			ENDCG
		}
	}
}
