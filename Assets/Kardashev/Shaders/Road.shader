// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Road"
{
	Properties
	{
		_Color0("Color 0", Color) = (0.8000001,0.1921569,0.1882353,1)
		_Texture0("Texture 0", 2D) = "white" {}
		_Tiling("Tiling", Float) = 2
		_Falloff("Falloff", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+1" "IgnoreProjector" = "True" }
		Cull Back
		Offset -1, -1
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha decal:blend 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _Texture0;
		uniform float _Tiling;
		uniform float _Falloff;


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= projNormal.x + projNormal.y + projNormal.z;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( topTexMap, tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( topTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( topTexMap, tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float4 appendResult47 = (float4(_Color0.r , _Color0.g , _Color0.b , 0));
			o.Albedo = appendResult47.xyz;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar31 = TriplanarSamplingSF( _Texture0, ase_worldPos, ase_worldNormal, _Falloff, _Tiling, 0 );
			float2 appendResult41 = (float2(triplanar31.x , triplanar31.z));
			float2 uv_TexCoord8 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float smoothstepResult10 = smoothstep( 0.4 , 0.7 , ( ( appendResult41 + 0.5 ) * uv_TexCoord8.x ).x);
			o.Alpha = saturate( smoothstepResult10 );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15201
135;138;2077;1025;1946.912;345.8878;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;33;-1417.026,-55.76392;Float;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;c25d4bd91552e1b488c1a68c58db62d1;c25d4bd91552e1b488c1a68c58db62d1;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1409.026,136.2361;Float;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1404.026,215.2362;Float;False;Property;_Falloff;Falloff;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;31;-1170.436,55.47895;Float;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;8;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;41;-802.9586,110.8538;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-835.9586,237.854;Float;False;Constant;_Float2;Float 2;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-895.9408,445.7235;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-663.9586,134.8538;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-552.9408,578.7235;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-554.9408,656.7235;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-428.6639,374.887;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;10;-219.0856,394.9298;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-773.2355,-74.30967;Float;False;Property;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.8000001,0.1921569,0.1882353,1;0.8000001,0.1921569,0.1882353,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;46;5.5875,349.6121;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-338.8125,-0.08776855;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;180.0812,77.43443;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Road;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;True;-1;-1;False;0;Transparent;0.5;True;False;1;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;-1;False;-1;-1;False;-1;-1;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;33;0
WireConnection;31;3;34;0
WireConnection;31;4;35;0
WireConnection;41;0;31;1
WireConnection;41;1;31;3
WireConnection;44;0;41;0
WireConnection;44;1;45;0
WireConnection;43;0;44;0
WireConnection;43;1;8;1
WireConnection;10;0;43;0
WireConnection;10;1;11;0
WireConnection;10;2;12;0
WireConnection;46;0;10;0
WireConnection;47;0;4;1
WireConnection;47;1;4;2
WireConnection;47;2;4;3
WireConnection;0;0;47;0
WireConnection;0;9;46;0
ASEEND*/
//CHKSM=13E370DB129828F03F96B57AA59CEB1426F3431D