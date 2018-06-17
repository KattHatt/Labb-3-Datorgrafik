uniform float4x4 World;
uniform float4x4 View;
uniform float4x4 Projection;
uniform float4x4 LightView;
uniform float4x4 LightProjection;

uniform float3 EyePosition;

uniform float3 DiffuseColor;
uniform float DiffuseIntensity;

uniform float3 SpecularColor;
uniform float SpecularIntensity;
uniform float SpecularPower;

uniform float3 AmbientColor;

uniform float3 LightDirection;

uniform float ShadowPower;

uniform float FogStart;
uniform float FogEnd;
uniform float3 FogColor;
uniform float FogEnabled;

struct VS_SHADOW_OUTPUT
{
	float4 Position : POSITION;
};

VS_SHADOW_OUTPUT RenderDepthMapVS(float4 position : POSITION)
{
	VS_SHADOW_OUTPUT output;
	float4x4 WorldViewProjection = mul(mul(World, LightView), LightProjection);
	output.Position = mul(position, WorldViewProjection);
	return output;
}

float4 RenderDepthMapPS(VS_SHADOW_OUTPUT input) : COLOR
{
	float4 color = input.Position.z / input.Position.w;
	return color.r;
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
	float3 Normal : NORMAL;
	float2 UV : TEXCOORD;
};

struct VS_OUTPUT
{
	// Projection space position
	float4 Position : POSITION0;
	// World space position
	float4 wPosition : POSITION1;
	float2 UV : TEXCOORD;
	float3 Normal : NORMAL;
};

float3 CalculateLightning(float4 position, float3 normal)
{
	float3 N = normalize(mul(normal, World));
	float3 V = normalize(EyePosition - position);
	float3 L = normalize(-LightDirection);
	float3 H = normalize(L + V);

	float3 ambient = AmbientColor;
	float3 diffuse = saturate(dot(N, L)) * DiffuseColor.rgb * DiffuseIntensity;
	float3 specular = pow(saturate(dot(N, H)), SpecularPower) * SpecularColor * SpecularIntensity;

	return ambient + specular + diffuse;
}

VS_OUTPUT RenderVS(VS_INPUT input)
{
	VS_OUTPUT output;
	float4x4 WorldViewProjection = mul(mul(World, View), Projection);
	output.Position = mul(input.Position, WorldViewProjection);
	output.wPosition = mul(input.Position, World);
	output.UV = input.UV;
	output.Normal = input.Normal;
	return output;
}

float4 RenderPS(VS_OUTPUT input) : COLOR
{
	float fogFactor = saturate((length(EyePosition - input.wPosition) - FogStart) / (FogEnd - FogStart));

	float3 textureColor = Texture.Sample(TextureSampler, input.UV).rgb;

	float3 color = CalculateLightning(input.wPosition, input.Normal) + textureColor.rgb;

	color.rgb = lerp(color.rgb, FogColor, fogFactor);

	return float4(color, 1);
}

technique Render
{
	pass Pass1
	{
		vertexshader = compile vs_4_0 RenderVS();
		pixelshader = compile ps_4_0 RenderPS();
	}
}