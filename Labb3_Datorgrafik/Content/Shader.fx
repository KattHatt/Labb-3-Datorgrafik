uniform float4x4 cameraMatrix;

uniform float4x4 World;
uniform float4x4 View;
uniform float4x4 Projection;

uniform float4 EyePosition;

uniform float3 DiffuseColor;
uniform float3 SpecularColor;
uniform float3 SpecularPower;

uniform float ShadowPower;

uniform float3 AmbientColor;

uniform float FogStart;
uniform float FogEnd;
uniform float3 FogColor;

uniform float4x4 LightView;
uniform float4x4 LightProjection;

struct VS_SHADOW_OUTPUT
{
	float4 Position : POSITION;
	float Depth : TEXCOORD0;
};

VS_SHADOW_OUTPUT RenderDepthMapVS(float4 position : POSITION)
{
	VS_SHADOW_OUTPUT output;
	float4x4 WorldViewProjection = mul(mul(World, LightView), LightProjection);
	output.Position = mul(position, WorldViewProjection);
	output.Depth = 1 - (output.Position.z / output.Position.w);
	return output;
}

float4 RenderDepthMapPS(VS_SHADOW_OUTPUT input) : COLOR
{
	return float4(1, 0, 0, 1);
}

float4 testPS(float4 position : POSITION0) : COLOR0
{
	return float4(1, 0, 0, 1);
}

float4 testVS(float4 position : POSITION0) : POSITION0
{
	float4 output;
	float4x4 WorldViewProjection = mul(mul(World, LightView), LightProjection);
	output = mul(position, WorldViewProjection);
	return output;
}

technique test
{
	pass Pass1
	{
		vertexshader = compile vs_4_0 testVS();
		pixelshader = compile ps_4_0 testPS();
	}
}

technique RenderDepthMap
{
	pass Pass1
	{
		vertexshader = compile vs_4_0 RenderDepthMapVS();
		pixelshader = compile ps_4_0 RenderDepthMapPS();
	}
}

Texture2D Texture;

SamplerState TextureSampler
{
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VS_INPUT
{
	float4 Position : POSITION;
	float2 UV : TEXCOORD;
};

struct VS_OUTPUT
{
	// Projection space position
	float4 Position : POSITION0;
	// World space position
	float4 wPosition : POSITION1;
	float2 UV : TEXCOORD;
};

VS_OUTPUT RenderVS(VS_INPUT input)
{
	VS_OUTPUT output;
	float4x4 WorldViewProjection = mul(mul(World, LightView), LightProjection);
	output.Position = mul(input.Position, WorldViewProjection);
	output.wPosition = mul(input.Position, World);
	output.UV = input.UV;
	return output;
}

float4 RenderPS(VS_OUTPUT input) : COLOR
{
	float fogFactor = saturate((length(EyePosition - input.wPosition) - FogStart) / (FogEnd - FogStart));

	float4 color = Texture.Sample(TextureSampler, input.UV);
	color.rgb = lerp(color.rgb, FogColor, fogFactor);
	return color;
}

technique Render
{
	pass Pass1
	{
		vertexshader = compile vs_4_0 RenderVS();
		pixelshader = compile ps_4_0 RenderPS();
	}
}