uniform float4x4 cameraMatrix;

float shadowmap_vs(float4 vertex : POSITION) : DEPTH
{
	return vertex.x;
}

float4 shadowmap_ps(float depth : DEPTH) : COLOR
{
	return float4(0, 0, 0, 0);
}

technique shadowmap
{
	pass pass0
	{
		vertexshader = compile vs_4_0 shadowmap_vs();
		pixelshader = compile ps_4_0 shadowmap_ps();
	}
}