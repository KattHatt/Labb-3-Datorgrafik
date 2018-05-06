struct VS_INPUT
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
    float2 TexCoord : TEXCOORD;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float4 WorldPosition : POSITION;
    float3 Normal : NORMAL;
    float2 TexCoord : TEXCOORD;
};

struct NF3D_LIGHT_OMNIDIRECTIONAL
{
    float4 Diffuse;
    float4 Ambient;
    float3 Position;
    float3 Attitude;
    float Range;
    int BindSlot;
};


//--------------------------------------------------------------------------------------
// Constant Buffer Variables
//--------------------------------------------------------------------------------------
cbuffer CB_PROJECTION : register(b0)
{
    matrix Projection;
}

cbuffer CB_VIEW : register(b1)
{
    matrix View;
    float4 CameraPosition;
}

cbuffer CB_WORLD : register(b2)
{
    matrix World;
}

cbuffer Light : register(b5)
{
    NF3D_LIGHT_OMNIDIRECTIONAL light;
};


//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
VS_OUTPUT VS(VS_INPUT Input)
{
    VS_OUTPUT Output = (VS_OUTPUT)0;

    // Change the position vector to be 4 units for proper matrix calculations.
    Input.Position.w = 1.0f;

    // Aply the perspective to every vertex.
    Output.Position = mul(Input.Position, World);
    Output.WorldPosition = Output.Position;
    Output.Position = mul(Output.Position, View);
    Output.Position = mul(Output.Position, Projection);

    // Normalise the normal vector.
    Output.Normal = mul(Input.Normal, (float3x3)World);
    Output.Normal = normalize(Output.Normal);

    // Transfer the texture coordinates.
    Output.TexCoord = Input.TexCoord;

    return Output;
}


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
float4 PS(VS_OUTPUT Input) : SV_Target
{
    // https://brooknovak.wordpress.com/2008/11/13/hlsl-per-pixel-point-light-using-phong-blinn-lighting-model/
    // Phong relfection is ambient + light-diffuse + spec highlights.
    // I = Ia*ka*Oda + fatt*Ip[kd*Od(N.L) + ks(R.V)^n]
    // Ref: http://www.whisqu.se/per/docs/graphics8.htm
    // and http://en.wikipedia.org/wiki/Phong_shading

    // Get light direction for this fragment
    float3 lightDir = normalize(light.Position - Input.WorldPosition);

    // Note: Non-uniform scaling not supported
    float diffuseLighting = saturate(dot(Input.Normal, -lightDir)); // per pixel diffuse lighting

    // Introduce fall-off of light intensity
    diffuseLighting *= ((length(lightDir) * length(lightDir)) / dot(light.Position - Input.WorldPosition, light.Position - Input.WorldPosition));

    // Using Blinn half angle modification for perofrmance over correctness
    float3 h = normalize(normalize(CameraPosition.xyz - Input.WorldPosition) - lightDir);
    float specLighting = pow(saturate(dot(h, Input.Normal)), 2.0f);

    return saturate(light.Ambient + (light.Diffuse * diffuseLighting * 0.6f) + (specLighting * 0.5f));
}