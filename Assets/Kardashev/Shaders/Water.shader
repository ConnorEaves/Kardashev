// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0.2509804,0.4117647,1,0.3333333)
		_Speed("Speed", Float) = 1
		_Texture0("Texture 0", 2D) = "white" {}
		_TextureScale("Texture Scale", Float) = 0.25
		_BlendWaveScale("Blend Wave Scale", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Texture0;
		uniform float _Speed;
		uniform float _TextureScale;
		uniform float _BlendWaveScale;
		uniform float4 _MainColor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float speed226 = _Speed;
			float mulTime141 = _Time.y * speed226;
			float2 appendResult145 = (float2(mulTime141 , 0));
			float3 ase_worldPos = i.worldPos;
			float3 break142 = ase_worldPos;
			float2 appendResult147 = (float2(break142.y , break142.z));
			float textureScale227 = _TextureScale;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 temp_cast_0 = (0.25).xxx;
			float3 temp_cast_1 = (2.0).xxx;
			float3 temp_output_157_0 = pow( saturate( ( abs( ase_vertexNormal ) - temp_cast_0 ) ) , temp_cast_1 );
			float3 break158 = temp_output_157_0;
			float3 break161 = ( temp_output_157_0 / ( break158.x + break158.y + break158.z ) );
			float2 appendResult146 = (float2(0 , mulTime141));
			float2 appendResult143 = (float2(break142.x , break142.z));
			float2 appendResult144 = (float2(break142.x , break142.y));
			float4 break171 = ( ( tex2D( _Texture0, ( ( appendResult145 + appendResult147 ) * textureScale227 ) ) * break161.x ) + ( tex2D( _Texture0, ( ( appendResult146 + appendResult143 ) * textureScale227 ) ) * break161.y ) + ( tex2D( _Texture0, ( ( appendResult145 + appendResult144 ) * textureScale227 ) ) * break161.z ) );
			float mulTime236 = _Time.y * speed226;
			float3 break235 = ase_worldPos;
			float2 appendResult237 = (float2(break235.y , break235.z));
			float2 break273 = appendResult237;
			float blendWaveScale251 = _BlendWaveScale;
			float mulTime210 = _Time.y * speed226;
			float2 appendResult207 = (float2(0 , mulTime210));
			float3 break203 = ase_worldPos;
			float2 appendResult208 = (float2(break203.y , break203.z));
			float3 temp_cast_2 = (0.25).xxx;
			float3 temp_cast_3 = (2.0).xxx;
			float3 temp_output_193_0 = pow( saturate( ( abs( ase_vertexNormal ) - temp_cast_2 ) ) , temp_cast_3 );
			float3 break194 = temp_output_193_0;
			float3 break197 = ( temp_output_193_0 / ( break194.x + break194.y + break194.z ) );
			float2 appendResult206 = (float2(mulTime210 , 0));
			float2 appendResult204 = (float2(break203.x , break203.z));
			float2 appendResult205 = (float2(break203.x , break203.y));
			float4 break190 = ( ( tex2D( _Texture0, ( ( appendResult207 + appendResult208 ) * textureScale227 ) ) * break197.x ) + ( tex2D( _Texture0, ( ( appendResult206 + appendResult204 ) * textureScale227 ) ) * break197.y ) + ( tex2D( _Texture0, ( ( appendResult207 + appendResult205 ) * textureScale227 ) ) * break197.z ) );
			float blendWaveNoise287 = ( break171.g + break190.b );
			float temp_output_269_0 = sin( ( mulTime236 + ( ( break273.x + break273.y ) * blendWaveScale251 ) + blendWaveNoise287 ) );
			float3 temp_cast_4 = (0.25).xxx;
			float3 temp_cast_5 = (2.0).xxx;
			float3 temp_output_258_0 = pow( saturate( ( abs( ase_vertexNormal ) - temp_cast_4 ) ) , temp_cast_5 );
			float3 break259 = temp_output_258_0;
			float3 break262 = ( temp_output_258_0 / ( break259.x + break259.y + break259.z ) );
			float2 appendResult239 = (float2(break235.x , break235.z));
			float2 break277 = appendResult239;
			float temp_output_268_0 = sin( ( mulTime236 + ( ( break277.x + break277.y ) * blendWaveScale251 ) + blendWaveNoise287 ) );
			float2 appendResult241 = (float2(break235.x , break235.y));
			float2 break281 = appendResult241;
			float temp_output_267_0 = sin( ( mulTime236 + ( ( break281.x + break281.y ) * blendWaveScale251 ) + blendWaveNoise287 ) );
			float blendWave270 = ( ( ( temp_output_269_0 * temp_output_269_0 ) * break262.x ) + ( ( temp_output_268_0 * temp_output_268_0 ) * break262.y ) + ( ( temp_output_267_0 * temp_output_267_0 ) * break262.z ) );
			float lerpResult289 = lerp( break171.b , break171.a , blendWave270);
			float lerpResult290 = lerp( break190.r , break190.g , blendWave270);
			float waves220 = ( lerpResult289 + lerpResult290 );
			float smoothstepResult223 = smoothstep( 0.75 , 2.0 , waves220);
			float3 appendResult71 = (float3(_MainColor.r , _MainColor.g , _MainColor.b));
			o.Albedo = saturate( ( smoothstepResult223 + appendResult71 ) );
			o.Alpha = _MainColor.a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15201
2767;92;2076;1025;1761.657;1391.033;1;False;True
Node;AmplifyShaderEditor.CommentaryNode;173;-1383.761,-1147.999;Float;False;2206.025;848.9157;nosie1;35;178;180;179;149;150;148;141;140;147;146;145;144;143;142;153;151;152;156;161;160;159;158;157;155;154;171;164;166;165;162;167;168;169;229;232;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;182;-1381.495,-266.5531;Float;False;2206.025;848.9157;nosie 2;36;218;217;216;214;213;212;210;209;208;207;206;205;204;203;201;200;199;198;197;196;195;194;193;192;191;190;189;188;187;186;185;184;183;176;230;231;;1,1,1,1;0;0
Node;AmplifyShaderEditor.NormalVertexDataNode;200;-1321.261,392.0905;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;151;-1323.527,-489.3545;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;199;-1122.261,456.0909;Float;False;Constant;_Float11;Float 11;3;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-1124.527,-425.354;Float;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;201;-1105.259,388.0905;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;153;-1107.525,-493.3545;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;154;-961.5247,-490.3545;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;191;-959.259,391.0905;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-1378.293,-1454.199;Float;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;209;-1145.945,185.3655;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;192;-821.2566,392.0905;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;140;-1148.21,-696.0798;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;156;-815.7808,-417.9954;Float;False;Constant;_Float6;Float 6;0;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;155;-823.5221,-489.3545;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;229;-1055.61,-925.491;Float;False;226;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;226;-1213.765,-1456.043;Float;False;speed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;198;-813.5152,463.4497;Float;False;Constant;_Float10;Float 10;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;230;-1044.197,-50.47202;Float;False;226;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;203;-950.2338,187.9018;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleTimeNode;210;-862.0149,-41.59248;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;141;-864.2805,-923.0377;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;-670.5228,-489.3545;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;142;-952.4993,-693.5434;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PowerNode;193;-668.2573,392.0905;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;194;-503.2566,449.0903;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;146;-666.8641,-982.8301;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;147;-658.5941,-587.4878;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;207;-664.5986,-101.3848;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;145;-662.689,-886.1504;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;144;-656.78,-778.9775;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;205;-654.5145,102.4678;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;228;-1379.496,-1368.173;Float;False;Property;_TextureScale;Texture Scale;3;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;206;-660.4235,-4.705124;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;208;-656.3286,293.9574;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;143;-659.28,-685.3774;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;204;-657.0145,196.0679;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;158;-505.5222,-432.3546;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-1205.65,-1367.142;Float;False;textureScale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;212;-464.7204,-18.06207;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;231;-469.7274,299.5345;Float;False;227;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;195;-271.2564,450.0903;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;150;-464.8878,-799.9498;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;232;-462.1158,-579.2877;Float;False;227;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;214;-457.1102,175.5226;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;213;-462.6222,81.49543;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;149;-459.3759,-705.9227;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;148;-466.986,-899.5073;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;-273.522,-431.3546;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;218;-284.0381,-14.76116;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-289.7382,189.8392;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-284.5382,87.13917;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;-286.8038,-794.306;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;196;-141.2563,389.0905;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;160;-143.5219,-492.3545;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;176;-368.0421,-216.9138;Float;True;Property;_Texture0;Texture 0;2;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;180;-292.0038,-691.606;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;-286.3037,-896.2063;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;183;-65.83302,187.0244;Float;True;Property;_TextureSample9;Texture Sample 9;8;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;252;-1670.017,1320.736;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;197;-13.5893,378.5436;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;167;-77.90845,-1086.829;Float;True;Property;_TextureSample6;Texture Sample 6;8;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;169;-68.09852,-694.4208;Float;True;Property;_TextureSample8;Texture Sample 8;8;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;161;-15.85481,-502.9014;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldPosInputsNode;234;-1445.831,957.2512;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;185;-75.64289,-205.3841;Float;True;Property;_TextureSample11;Texture Sample 11;8;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;168;-68.68116,-889.4951;Float;True;Property;_TextureSample7;Texture Sample 7;8;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;184;-66.4156,-8.049911;Float;True;Property;_TextureSample10;Texture Sample 10;8;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;273.5096,-746.046;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;235;-1250.119,959.7876;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;268.7754,41.39917;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;275.7754,135.3993;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;273.3116,-60.39565;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;271.0457,-941.8409;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;253;-1471.017,1384.737;Float;False;Constant;_Float12;Float 12;3;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;254;-1454.015,1316.736;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;266.5096,-840.046;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;189;426.7756,7.752455;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;239;-956.9001,967.9536;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;241;-954.4001,874.3535;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;255;-1308.015,1319.736;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;424.5099,-873.6927;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;237;-956.2142,1065.843;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;256;-1170.012,1320.736;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;273;-770.7308,1125.108;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;257;-1162.271,1392.095;Float;False;Constant;_Float13;Float 13;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;250;-1376.975,-1282.283;Float;False;Property;_BlendWaveScale;Blend Wave Scale;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;190;565.3575,10.09297;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;171;563.0917,-871.3522;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;281;-788.1143,888.6578;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;277;-775.959,1014.632;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PowerNode;258;-1017.013,1320.736;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;282;-540.4969,882.1398;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;279;-528.3416,1008.114;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;245;-606.3999,1222.629;Float;False;251;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;233;-417.8825,777.0252;Float;False;226;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;286;1221.628,-230.773;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;274;-523.1134,1118.59;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;251;-1174.13,-1284.252;Float;False;blendWaveScale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;-281.0455,868.5954;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;236;-243.5003,778.1048;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;259;-852.0118,1377.736;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;280;-268.8902,994.5694;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;-263.662,1105.046;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;287;1358.462,-227.3809;Float;False;blendWaveNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;288;-263.0993,1216.022;Float;False;287;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;260;-620.0118,1378.736;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;284;52.27426,877.3733;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;276;48.85769,1131.424;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;278;48.4295,1006.547;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;269;189.4451,1106.376;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;268;192.6048,1012.112;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;267;184.8652,918.0571;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;261;-490.0118,1317.736;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;295;317.9973,1094.255;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;262;-362.3448,1307.189;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;294;319.2974,999.3555;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;293;318.6258,893.9617;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;538.6652,1088.573;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;543.5563,991.725;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;546.8353,1190.276;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;266;711.1151,1067.236;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;270;847.8058,1070.962;Float;False;blendWave;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;272;852.2124,-406.9728;Float;True;270;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;289;1213.195,-507.2852;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;290;1220.195,-389.1945;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;291;1400.195,-444.1945;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;1890.623,-231.373;Float;False;Property;_MainColor;Main Color;0;0;Create;True;0;0;False;0;0.2509804,0.4117647,1,0.3333333;0.2509804,0.4117647,1,0.3333333;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;224;1914.361,-401.5129;Float;False;Constant;_Float14;Float 14;10;0;Create;True;0;0;False;0;0.75;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;225;1932.329,-327.8507;Float;False;Constant;_Float15;Float 15;10;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;221;1903.567,-479.0614;Float;False;220;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;220;1605.654,-451.8329;Float;False;waves;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;223;2128.458,-348.8281;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;2156.519,-212.6804;Float;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;2304.533,-290.6986;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;27;2511.083,-294.346;Float;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2739.624,-297.3836;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;True;1;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;201;0;200;0
WireConnection;153;0;151;0
WireConnection;154;0;153;0
WireConnection;154;1;152;0
WireConnection;191;0;201;0
WireConnection;191;1;199;0
WireConnection;192;0;191;0
WireConnection;155;0;154;0
WireConnection;226;0;108;0
WireConnection;203;0;209;0
WireConnection;210;0;230;0
WireConnection;141;0;229;0
WireConnection;157;0;155;0
WireConnection;157;1;156;0
WireConnection;142;0;140;0
WireConnection;193;0;192;0
WireConnection;193;1;198;0
WireConnection;194;0;193;0
WireConnection;146;1;141;0
WireConnection;147;0;142;1
WireConnection;147;1;142;2
WireConnection;207;1;210;0
WireConnection;145;0;141;0
WireConnection;144;0;142;0
WireConnection;144;1;142;1
WireConnection;205;0;203;0
WireConnection;205;1;203;1
WireConnection;206;0;210;0
WireConnection;208;0;203;1
WireConnection;208;1;203;2
WireConnection;143;0;142;0
WireConnection;143;1;142;2
WireConnection;204;0;203;0
WireConnection;204;1;203;2
WireConnection;158;0;157;0
WireConnection;227;0;228;0
WireConnection;212;0;207;0
WireConnection;212;1;205;0
WireConnection;195;0;194;0
WireConnection;195;1;194;1
WireConnection;195;2;194;2
WireConnection;150;0;146;0
WireConnection;150;1;143;0
WireConnection;214;0;207;0
WireConnection;214;1;208;0
WireConnection;213;0;206;0
WireConnection;213;1;204;0
WireConnection;149;0;145;0
WireConnection;149;1;147;0
WireConnection;148;0;145;0
WireConnection;148;1;144;0
WireConnection;159;0;158;0
WireConnection;159;1;158;1
WireConnection;159;2;158;2
WireConnection;218;0;212;0
WireConnection;218;1;231;0
WireConnection;217;0;214;0
WireConnection;217;1;231;0
WireConnection;216;0;213;0
WireConnection;216;1;231;0
WireConnection;179;0;150;0
WireConnection;179;1;232;0
WireConnection;196;0;193;0
WireConnection;196;1;195;0
WireConnection;160;0;157;0
WireConnection;160;1;159;0
WireConnection;180;0;149;0
WireConnection;180;1;232;0
WireConnection;178;0;148;0
WireConnection;178;1;232;0
WireConnection;183;0;176;0
WireConnection;183;1;217;0
WireConnection;197;0;196;0
WireConnection;167;0;176;0
WireConnection;167;1;178;0
WireConnection;169;0;176;0
WireConnection;169;1;180;0
WireConnection;161;0;160;0
WireConnection;185;0;176;0
WireConnection;185;1;218;0
WireConnection;168;0;176;0
WireConnection;168;1;179;0
WireConnection;184;0;176;0
WireConnection;184;1;216;0
WireConnection;166;0;167;0
WireConnection;166;1;161;2
WireConnection;235;0;234;0
WireConnection;187;0;184;0
WireConnection;187;1;197;1
WireConnection;188;0;185;0
WireConnection;188;1;197;2
WireConnection;186;0;183;0
WireConnection;186;1;197;0
WireConnection;162;0;169;0
WireConnection;162;1;161;0
WireConnection;254;0;252;0
WireConnection;165;0;168;0
WireConnection;165;1;161;1
WireConnection;189;0;186;0
WireConnection;189;1;187;0
WireConnection;189;2;188;0
WireConnection;239;0;235;0
WireConnection;239;1;235;2
WireConnection;241;0;235;0
WireConnection;241;1;235;1
WireConnection;255;0;254;0
WireConnection;255;1;253;0
WireConnection;164;0;162;0
WireConnection;164;1;165;0
WireConnection;164;2;166;0
WireConnection;237;0;235;1
WireConnection;237;1;235;2
WireConnection;256;0;255;0
WireConnection;273;0;237;0
WireConnection;190;0;189;0
WireConnection;171;0;164;0
WireConnection;281;0;241;0
WireConnection;277;0;239;0
WireConnection;258;0;256;0
WireConnection;258;1;257;0
WireConnection;282;0;281;0
WireConnection;282;1;281;1
WireConnection;279;0;277;0
WireConnection;279;1;277;1
WireConnection;286;0;171;1
WireConnection;286;1;190;2
WireConnection;274;0;273;0
WireConnection;274;1;273;1
WireConnection;251;0;250;0
WireConnection;283;0;282;0
WireConnection;283;1;245;0
WireConnection;236;0;233;0
WireConnection;259;0;258;0
WireConnection;280;0;279;0
WireConnection;280;1;245;0
WireConnection;275;0;274;0
WireConnection;275;1;245;0
WireConnection;287;0;286;0
WireConnection;260;0;259;0
WireConnection;260;1;259;1
WireConnection;260;2;259;2
WireConnection;284;0;236;0
WireConnection;284;1;283;0
WireConnection;284;2;288;0
WireConnection;276;0;236;0
WireConnection;276;1;275;0
WireConnection;276;2;288;0
WireConnection;278;0;236;0
WireConnection;278;1;280;0
WireConnection;278;2;288;0
WireConnection;269;0;276;0
WireConnection;268;0;278;0
WireConnection;267;0;284;0
WireConnection;261;0;258;0
WireConnection;261;1;260;0
WireConnection;295;0;269;0
WireConnection;295;1;269;0
WireConnection;262;0;261;0
WireConnection;294;0;268;0
WireConnection;294;1;268;0
WireConnection;293;0;267;0
WireConnection;293;1;267;0
WireConnection;265;0;294;0
WireConnection;265;1;262;1
WireConnection;264;0;295;0
WireConnection;264;1;262;0
WireConnection;263;0;293;0
WireConnection;263;1;262;2
WireConnection;266;0;264;0
WireConnection;266;1;265;0
WireConnection;266;2;263;0
WireConnection;270;0;266;0
WireConnection;289;0;171;2
WireConnection;289;1;171;3
WireConnection;289;2;272;0
WireConnection;290;0;190;0
WireConnection;290;1;190;1
WireConnection;290;2;272;0
WireConnection;291;0;289;0
WireConnection;291;1;290;0
WireConnection;220;0;291;0
WireConnection;223;0;221;0
WireConnection;223;1;224;0
WireConnection;223;2;225;0
WireConnection;71;0;62;1
WireConnection;71;1;62;2
WireConnection;71;2;62;3
WireConnection;64;0;223;0
WireConnection;64;1;71;0
WireConnection;27;0;64;0
WireConnection;0;0;27;0
WireConnection;0;9;62;4
ASEEND*/
//CHKSM=6DC6EA5D30A0AB1E32D040A887C781E1669DC5AE