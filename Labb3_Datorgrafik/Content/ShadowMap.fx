float4x4 xWorldViewProjection;
float4x4 xLightsWorldViewProjection;
float4x4 xWorld;
float3 xLightPos;
float xLightPower;
float xAmbient;

Texture xTexture;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };
struct VertexToPixel
{
	float4 Position     : POSITION;
	float2 TexCoords    : TEXCOORD0;
	float3 Normal        : TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};

struct PixelToFrame
{
	float4 Color        : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
	return dot(-lightDir, normal);
}

VertexToPixel SimplestVertexShader(float4 inPos : POSITION0, float3 inNormal : NORMAL0, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;

	Output.Position = mul(inPos, xWorldViewProjection);
	Output.TexCoords = inTexCoords;
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);

	return Output;
}

PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= xLightPower;

	PSIn.TexCoords.y--;
	float4 baseColor = tex2D(TextureSampler, PSIn.TexCoords);
	Output.Color = baseColor * (diffuseLightingFactor + xAmbient);

	return Output;
}

technique Simplest
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 SimplestVertexShader();
		PixelShader = compile ps_4_0 OurFirstPixelShader();
	}
}


struct SMapVertexToPixel
{
	float4 Position     : POSITION;
	float4 Position2D    : TEXCOORD0;
};

struct SMapPixelToFrame
{
	float4 Color : COLOR0;
};


SMapVertexToPixel ShadowMapVertexShader(float4 inPos : POSITION)
{
	SMapVertexToPixel Output = (SMapVertexToPixel)0;

	Output.Position = mul(inPos, xLightsWorldViewProjection);
	Output.Position2D = Output.Position;

	return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
	SMapPixelToFrame Output = (SMapPixelToFrame)0;

	Output.Color = PSIn.Position2D.z / PSIn.Position2D.w;

	return Output;
}


technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 ShadowMapVertexShader();
		PixelShader = compile ps_4_0 ShadowMapPixelShader();
	}
}