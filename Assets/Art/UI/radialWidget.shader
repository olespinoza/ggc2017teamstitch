Shader "UICustom/RadialWidget"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GradientTex ("Gradient", 2D) = "white" {}
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

			uniform float _sample[256];

			sampler2D _MainTex;
			sampler2D _GradientTex;
			float4 _MainTex_ST;
			float4 _GradientTex_ST;

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
				o.uv2 = TRANSFORM_TEX(v.uv, _GradientTex);
				return o;
			}

			half4 frag( v2f i ) : COLOR
			{
				half4 color = tex2D(_MainTex, i.uv.xy);
				half4 gradColor = tex2D(_GradientTex, i.uv2.xy);
				float alpha = color.a * _sample[floor(gradColor.r*255)];
				return half4(color.r, color.g, color.b, alpha);
			}
			ENDCG
		}
	}

	Fallback "Transparent/Diffuse"
}
