/*********************************************************************NVMH3****
$Revision: #3 $

Copyright NVIDIA Corporation 2007
TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THIS SOFTWARE IS PROVIDED
*AS IS* AND NVIDIA AND ITS SUPPLIERS DISCLAIM ALL WARRANTIES, EITHER EXPRESS
OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL NVIDIA OR ITS SUPPLIERS
BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT, OR CONSEQUENTIAL DAMAGES
WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS,
BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY
LOSS) ARISING OUT OF THE USE OF OR INABILITY TO USE THIS SOFTWARE, EVEN IF
NVIDIA HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.

% Gooch shading w/glossy hilight in HLSL ps_2 pixel shader.
% Textured and non-textued versions are supplied.

keywords: material stylized classic


To learn more about shading, shaders, and to bounce ideas off other shader
    authors and users, visit the NVIDIA Shader Library Forums at:

    http://developer.nvidia.com/forums/

******************************************************************************/

#ifdef _3DSMAX_
int ParamID = 0x0003; // Defined by Autodesk
#endif /* _3DSMAX_ */

float Script : STANDARDSGLOBAL <
    string UIWidget = "none";
    string ScriptClass = "object";
    string ScriptOrder = "standard";
    string ScriptOutput = "color";
    string Script = "Technique=Technique?Untextured:Textured;";
> = 0.8;

//// UN-TWEAKABLES - AUTOMATICALLY-TRACKED TRANSFORMS ////////////////

float4x4 WorldITXf : WorldInverseTranspose < string UIWidget="None"; >;
float4x4 WvpXf : WorldViewProjection < string UIWidget="None"; >;
float4x4 WorldXf : World < string UIWidget="None"; >;
float4x4 ViewIXf : ViewInverse < string UIWidget="None"; >;

/************* TWEAKABLES **************/

float3 Lamp0Pos : Position <
    string Object = "PointLight0";
    string UIName =  "Lamp 0 Position";
    string Space = "World";
> = {-0.5f,2.0f,1.25f};

float3 LiteColor <
    string UIName =  "Lite Surface";
    string UIWidget = "Color";
> = {0.8f, 0.5f, 0.1f};

float3 DarkColor <
    string UIName =  "Dark Surface";
    string UIWidget = "Color";
> = {0.0f, 0.0f, 0.0f};

float3 WarmColor <
    string UIName =  "Gooch Warm Tone";
    string UIWidget = "Color";
> = {0.5f, 0.4f, 0.05f};

float3 CoolColor <
    string UIName =  "Gooch Cool Tone";
    string UIWidget = "Color";
> = {0.05f, 0.05f, 0.6f};

float3 SpecColor : Specular <
    string UIName =  "Hilight";
    string UIWidget = "Color";
> = {0.7f, 0.7f, 1.0f};

float SpecExpon : SpecularPower <
    string UIWidget = "slider";
    float UIMin = 1.0;
    float UIMax = 128.0;
    float UIStep = 1.0;
    string UIName =  "Specular Exponent";
> = 40.0;

float GlossTop <
    string UIWidget = "slider";
    float UIMin = 0.2;
    float UIMax = 1.0;
    float UIStep = 0.05;
    string UIName =  "Maximum for Gloss Dropoff";
> = 0.7;

float GlossBot
<
    string UIWidget = "slider";
    float UIMin = 0.05;
    float UIMax = 0.95;
    float UIStep = 0.05;
    string UIName =  "Minimum for Gloss Dropoff";
> = 0.5;

float GlossDrop
<
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.05;
    string UIName =  "Strength of Glossy Dropoff";
> = 0.2;

texture ColorTexture : DIFFUSE <
    string ResourceName = "default_color.dds";
    string UIName =  "Diffuse Texture";
    string ResourceType = "2D";
>;

sampler2D ColorSampler = sampler_state {
    Texture = <ColorTexture>;
    MinFilter = Linear;
    MipFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};  

// shared shadow mapping supported in Cg version

//////////////////////////

/* data from application vertex buffer */
struct appdata {
    float3 Position	: POSITION;
    float4 UV		: TEXCOORD0;
    float4 Normal	: NORMAL;
    float4 Tangent	: TANGENT0;
    float4 Binormal	: BINORMAL0;
};

/* data passed from vertex shader to pixel shader */
struct vertexOutput {
    float4 HPosition	: POSITION;
    float2 UV		: TEXCOORD0;
    // The following values are passed in "World" coordinates since
    //   it tends to be the most flexible and easy for handling
    //   reflections, sky lighting, and other "global" effects.
    float3 LightVec	: TEXCOORD1;
    float3 WorldNormal	: TEXCOORD2;
    float3 WorldTangent	: TEXCOORD3;
    float3 WorldBinormal : TEXCOORD4;
    float3 WorldView	: TEXCOORD5;
};

/*********** vertex shader ******/

/*********** Generic Vertex Shader ******/

vertexOutput std_VS(appdata IN) {
    vertexOutput OUT = (vertexOutput)0;
    OUT.WorldNormal = mul(IN.Normal,WorldITXf).xyz;
    OUT.WorldTangent = mul(IN.Tangent,WorldITXf).xyz;
    OUT.WorldBinormal = mul(IN.Binormal,WorldITXf).xyz;
    float4 Po = float4(IN.Position.xyz,1);
    float4 Pw = mul(Po,WorldXf);
    OUT.LightVec = (Lamp0Pos - Pw.xyz);
#ifdef FLIP_TEXTURE_Y
    OUT.UV = float2(IN.UV.x,(1.0-IN.UV.y));
#else /* !FLIP_TEXTURE_Y */
    OUT.UV = IN.UV.xy;
#endif /* !FLIP_TEXTURE_Y */
    OUT.WorldView = normalize(ViewIXf[3].xyz - Pw.xyz);
    OUT.HPosition = mul(Po,WvpXf);
    return OUT;
}

/*********** pixel shader ******/

static float g_Min = min(GlossBot,GlossTop);
static float g_Max = max(GlossBot,GlossTop);
static float g_Dr = (1.0 - GlossDrop);

void gooch_shared(vertexOutput IN,
		out float4 DiffuseContrib,
		out float4 SpecularContrib)
{
    float3 Ln = normalize(IN.LightVec.xyz);
    float3 Nn = normalize(IN.WorldNormal);
    float3 Vn = normalize(IN.WorldView);
    float3 Hn = normalize(Vn + Ln);
    float hdn = pow(max(0,dot(Hn,Nn)),SpecExpon);
    hdn = hdn * (GlossDrop+smoothstep(g_Min,g_Max,hdn)*g_Dr);
    float ldn = dot(Ln,Nn);
    SpecularContrib = float4((hdn * SpecColor),1);
    float mixer = 0.5 * (ldn + 1.0);
    float3 surfColor = lerp(DarkColor,LiteColor,mixer);
    float3 toneColor = lerp(CoolColor,WarmColor,mixer);
    DiffuseContrib = float4((surfColor + toneColor),1);
}

float4 gooch_PS(vertexOutput IN) :COLOR
{
	float4 diffContrib;
	float4 specContrib;
	gooch_shared(IN,diffContrib,specContrib);
    float4 result = diffContrib + specContrib;
    return result;
}

float4 goochT_PS(vertexOutput IN) :COLOR
{
	float4 diffContrib;
	float4 specContrib;
	gooch_shared(IN,diffContrib,specContrib);
    float4 result = tex2D(ColorSampler,IN.UV.xy)*diffContrib + specContrib;
    return result;
}

/*************/

technique Gooch <
	string Script = "Pass=p0;";
> {
	pass p0 <
	string Script = "Draw=geometry;";
    > {		
        VertexShader = compile vs_2_0 std_VS();
		ZEnable = true;
		ZWriteEnable = true;
		ZFunc = LessEqual;
		AlphaBlendEnable = false;
		CullMode = None;
        PixelShader = compile ps_2_a gooch_PS();
    }
}

technique Textured <
	string Script = "Pass=p0;";
> {
	pass p0 <
	string Script = "Draw=geometry;";
    > {		
        VertexShader = compile vs_2_0 std_VS();
		ZEnable = true;
		ZWriteEnable = true;
		ZFunc = LessEqual;
		AlphaBlendEnable = false;
		CullMode = None;
        PixelShader = compile ps_2_a goochT_PS();
    }
}

/***************************** eof ***/
