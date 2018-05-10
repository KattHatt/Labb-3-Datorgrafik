float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection = normalize(float3(-1, -1, -1));
float3 DiffuseLight = 1.25;
float3 AmbientLight = 0.25;
uniform const float3    DiffuseColor = 1;
uniform const float     Alpha = 1;
uniform const float3    EmissiveColor = 0;
uniform const float3    SpecularColor = 1;
uniform const float     SpecularPower = 16;
uniform const float3    EyePosition;

uniform const float     FogEnabled;
uniform const float     FogStart;
uniform const float     FogEnd;
uniform const float3    FogColor;
float3 cameraPos : CAMERAPOS;
texture Texture;

sampler Sampler = sampler_state
{
	Texture = (Texture);
};

struct CommonVSOutput
{
	float4  Pos_ws;
	float4  Pos_ps;
	float4  Diffuse;
	float3  Specular;
	float   FogFactor;
};

struct VertexLightingVSOutputTx
{
	float4  PositionPS  : POSITION;
	float4  Diffuse     : COLOR0;
	float4  Specular    : COLOR1;
	float2  TexCoord    : TEXCOORD0;
};

struct VertexLightingPSInputTx
{
	float4  Diffuse     : COLOR0;
	float4  Specular    : COLOR1;
	float2  TexCoord    : TEXCOORD0;
};

struct VSInputTx
{
	float4  Position    : POSITION;
	float2  TexCoord    : TEXCOORD0;
};

float ComputeFogFactor(float d)
{
	return clamp((d - FogStart) / (FogEnd - FogStart), 0, 1) * FogEnabled;
}

CommonVSOutput ComputeCommonVSOutput(float4 position)
{
	CommonVSOutput vout;

	float4 pos_ws = mul(position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);

	vout.Pos_ws = pos_ws;
	vout.Pos_ps = pos_ps;

	vout.Diffuse = float4(DiffuseColor.rgb + EmissiveColor, Alpha);
	vout.Specular = 0;
	vout.FogFactor = ComputeFogFactor(length(EyePosition - pos_ws));

	return vout;
}

VertexLightingVSOutputTx VSBasicTx(VSInputTx vin)
{
	VertexLightingVSOutputTx vout;

	CommonVSOutput cout = ComputeCommonVSOutput(vin.Position);

	vout.PositionPS = cout.Pos_ps;
	vout.Diffuse = cout.Diffuse;
	vout.Specular = float4(cout.Specular, cout.FogFactor);
	vout.TexCoord = vin.TexCoord;

	return vout;
}

float4 PSBasicTx(VertexLightingPSInputTx pin) : COLOR
{
	float4 color = tex2D(Sampler, pin.TexCoord) * pin.Diffuse + float4(pin.Specular.rgb, 0);
	color.rgb = lerp(color.rgb, FogColor, pin.Specular.w);
	return color;
}

// For rendering without instancing.
technique Fog
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VSBasicTx();
		PixelShader = compile ps_4_0 PSBasicTx();
	}
}