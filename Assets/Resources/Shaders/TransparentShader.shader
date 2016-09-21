Shader "Custom/TransparentShader" {
	Properties {
		_Tint ("ColorTint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent"}
		AlphaTest Greater 0
		ZWrite Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Tint;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Tint.rgb;
			o.Alpha = c.a * _Tint.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
