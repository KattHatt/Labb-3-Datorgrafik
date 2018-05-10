
float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float3 xLightPosition;
float3 xConeDirection;
float xLightStrength;
float xAmbient;
float xConeAngle;
float xConeDecay;


struct SLVertexToPixel
{
	float4 Position: POSITION;
	float3 Normal: TEXCOORD0;
	float LightDirection : TEXCOORD1;
};

struct SLPixelToFrame 
{
	float4 Color : COLOR0;
};

SLVertexToPixel SLVertexShader(float4 inPos: POSITION0, float3 inNormal : NORMAL0)
{
	SLVertexToPixel Output = (SLVertexToPixel)0;

	float4x4 preViewProjection = mul(xView, xProjection);
	float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);
	float3 final3DPos = mul(inPos, xWorld);
	float3x3 rotMatrix = (float3x3)xWorld;
	float3 rotNormal = mul(inNormal, rotMatrix);

	Output.Position = mul(inPos, preWorldViewProjection);
	Output.LightDirection = final3DPos - xLightPosition;
	Output.Normal = rotNormal;

	return Output;
}


SLPixelToFrame SLPixelShader(SLVertexToPixel PSIn) : COLOR0
{
	SLPixelToFrame Output = (SLPixelToFrame)0;

	float4 baseColor = float4(0, 0, 1, 1);
	float3 normal = normalize(PSIn.Normal);
	float3 lightDirection = normalize(PSIn.LightDirection);
	float coneDot = dot(lightDirection, normalize(xConeDirection));
	float shading = 0;
	
	if (coneDot > xConeAngle)
	{
		float coneAttenuation = pow(coneDot, xConeDecay);
		shading = dot(normal, -lightDirection);
		shading *= xLightStrength;
		shading *= coneAttenuation;
	}

	Output.Color = baseColor * (shading + xAmbient);

	return Output;
}

technique SpotLight
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 SLVertexShader();
		PixelShader = compile ps_4_0 SLPixelShader();
	}
}