Shader "Unlit/MovieShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Culling Mask", 2D) = "white" {}
		_Cutoff ("Texture", Range(0,1)) = .5
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Mask;
			float4 _MainTex_ST;
			float4 _Mask_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			v2f vert ( appdata v )
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _Mask);
				return o;
			}

			half4 frag( v2f i ) : COLOR
			{
				half4 color = tex2D(_MainTex, i.uv.xy);
				half4 color2 = tex2D(_Mask, i.uv2.xy);
				 
				return half4(color.r, color.g, color.b, color2.r);
			}
			ENDCG
		}
	}

	Fallback "Transparent/Diffuse"
}
