uniform float4x4 cameraMatrix;

uniform float4x4 World;
uniform float4x4 View;
uniform float4x4 Projection;

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

struct PS_INPUT
{
	float4 Position : POSITION;
	float2 UV : TEXCOORD;
};

struct VS_INPUT
{
	float4 Position : POSITION;
	float2 UV : TEXCOORD;
};

PS_INPUT RenderVS(VS_INPUT input)
{
	PS_INPUT output;
	float4x4 WorldViewProjection = mul(mul(World, LightView), LightProjection);
	output.Position = mul(input.Position, WorldViewProjection);
	output.UV = input.UV;
	return output;
}

float4 RenderPS(PS_INPUT input) : COLOR
{
	float4 color = Texture.Sample(TextureSampler, input.UV);
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