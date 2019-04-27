// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/Water" {
	Properties {
		_Color ("Main Color", Color) = (0,0,0,0)
		_Color2 ("Main Color", Color) = (0,0,0,0)
		_ColorLerp ("Color Lerp Height", Range(0.1, 100)) = 25
		_ColorLerpBase ("Color Lerp Base", Range(0, 100)) = 10

		_CloudMap ("Cloud Map", 2D) = "gray" {}
		_Zoom ("Cloud Detail", Range(-1, 1)) = 0
		_Effect ("Effect Depth", Range(-0.5, 0.5)) = 0
		_Speed ("Speed", Range(0, 5)) = 2
		
		
		_VertDistTex ("Vertex Distort Map", 2D) = "gray" {}
		_VertDistDens ("Density", range(0, 10)) = 0
		_VertDistAmount ("Amount", range(-10, 10)) = 0
		_VertDistSpeed ("Speed", range(0, 10)) = 0
	}
	SubShader {
			Tags { "RenderType"="Opaque" }
			LOD 150
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert noforwardadd
		
		fixed4 _Color;
		fixed4 _Color2;

		uniform float _ColorLerp;
		uniform float _ColorLerpBase;

		sampler2D _CloudMap;
		uniform float _Zoom;
		uniform float _Effect;
		uniform float _Speed;
		
		
		sampler2D _VertDistTex;
		uniform float _VertDistDens;
		uniform float _VertDistAmount;
		uniform float _VertDistSpeed;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos; // tohle se plni samo a nepotrebuje vert funkci
			float3 localPos;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			
			float4 vo;
			vo = tex2Dlod(_VertDistTex, v.vertex * _VertDistDens);
			v.vertex.z += sin((_Time.y  * _VertDistSpeed) + vo.r + o.uv_MainTex.y) * _VertDistAmount;
			v.vertex.z -= sin((_Time.y  * _VertDistSpeed) + vo.g + o.uv_MainTex.x) * _VertDistAmount;
			o.localPos = v.vertex.xyz;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = _Color;
			fixed4 c2 = _Color2;

			fixed4 cloud = tex2D( _CloudMap, IN.worldPos.xz * _Zoom + _Time.x * _Speed);
			
			
			o.Albedo = lerp(c.rgb, c2.rgb, clamp((IN.worldPos.y + _ColorLerpBase) / _ColorLerp, 0.0f, 1.1f));
			
			o.Albedo = o.Albedo + ((1 - cloud.r * 2) * _Effect);
			//o.Albedo = c.rgb + ((1 - cloud.r * 2) * _Effect);
			o.Alpha = 0;
		}
		ENDCG
	}
	
	Fallback "Mobile/VertexLit"
}
