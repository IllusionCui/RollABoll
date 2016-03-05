Shader "ball/food"
 {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_ColorTint ("Tint", Color) = (1.0,1.0,1.0, 0.0)
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Float) = 3
		_EmissionPower ("Emission Power", Float) = 0.63
		_Cutoff ("Cutout", Float) = 0.5
	}
	SubShader {
		Tags {
			"Queue"="Transparent-1"
			"IgnoreProjector"="true"
			"RenderType"="Transparent"
			"Reflection" = "RenderReflectionTransparentBlend" 
		}
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf CustomSkin noforwardadd alphatest:_Cutoff
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		sampler2D _MainTex;
		fixed4 _ColorTint;
		fixed4 _RimColor;
		fixed _RimPower;
		fixed _EmissionPower;

		half4 LightingCustomSkin (SurfaceOutput s, fixed3 lightDir, fixed atten)
	  	{	  
		  	half pi = 3.14159265;		
			fixed diff = max (0, dot (s.Normal, lightDir));						
			half3 nrmDiff = ((diff*s.Albedo*_LightColor0.rgb)/half3(pi))*atten*2;
		
			half4 c;
			c.rgb = nrmDiff;
			c.a = s.Alpha;
			return c;		
		}

		//half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten) {
		//	half NdotL = dot (s.Normal, lightDir);
		//	half diff = NdotL * 0.5 + 0.5;
		//	half4 c;
		//	c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
		//	c.a = s.Alpha;
		//	return c;
		//}

		void surf (Input IN, inout SurfaceOutput o) {	
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);		
			fixed rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

			o.Albedo = c.rgb * _ColorTint.rgb;
			o.Emission = _RimColor.rgb * pow (rim, _RimPower) + c.rgb* _EmissionPower;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	Fallback "Diffuse"
}