Shader "HealthMagic"
{
	Properties 
	{
_Color("Bar Color", Color) = (1,1,1,1)
_MainTex("Main Texture", 2D) = "black" {}
_Effect("Distortion Texture", 2D) = "black" {}
_Mask("Cutoff Mask", 2D) = "black" {}
_Distortion("Distortion Power", Float) = 0
_Offset("Distortion Offset", Float) = 0
_Speed("Scroll Speed", Float) = 0
_Cutoff("Bar Cutoff", Range(0,1.5) ) = 0.5

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
Mode Off
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  noambient nolightmap alpha decal:blend vertex:vert
#pragma target 2.0


float4 _Color;
sampler2D _MainTex;
sampler2D _Effect;
sampler2D _Mask;
float _Distortion;
float _Offset;
float _Speed;
float _Cutoff;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_Effect;
float2 uv_MainTex;
float4 color : COLOR;
float2 uv_Mask;

			};

			void vert (inout appdata_full v, out Input o) {
UNITY_INITIALIZE_OUTPUT(Input, o);
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Split0=(IN.uv_Effect.xyxy);
float4 Multiply5=_Speed.xxxx * _Time;
float4 Add2=float4( Split0.x, Split0.x, Split0.x, Split0.x) + Multiply5;
float4 Assemble0=float4(Add2.x, float4( Split0.y, Split0.y, Split0.y, Split0.y).y, float4( Split0.z, Split0.z, Split0.z, Split0.z).z, float4( Split0.w, Split0.w, Split0.w, Split0.w).w);
float4 Floor0=floor(Assemble0);
float4 Subtract2=Assemble0 - Floor0;
float4 Tex2D3=tex2D(_Effect,Subtract2.xy);
float4 Multiply4=Tex2D3 * _Distortion.xxxx;
float4 Add1=(IN.uv_Effect.xyxy) + Multiply4;
float4 Multiply1=_Offset.xxxx * _Time;
float4 Subtract1=Add1 - Multiply1;
float4 Tex2D2=tex2D(_Effect,Subtract1.xy);
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Add0=Tex2D2 + Sampled2D0;
float4 Multiply0=_Color * Add0;
float4 Multiply6=Multiply0 * IN.color;
float4 SplatAlpha0=IN.color.w;
float4 Sampled2D2=tex2D(_Mask,IN.uv_Mask.xy);
float4 Split1=Sampled2D2;
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - float4( Split1.x, Split1.x, Split1.x, Split1.x);
float4 Subtract3=_Cutoff.xxxx - Invert1;
float4 Multiply3=Subtract3 * float4( 9,9,9,9 );
float4 Multiply2=Sampled2D0.aaaa * Multiply3;
float4 Multiply7=SplatAlpha0 * Multiply2;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply6;
o.Alpha = Multiply7;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}