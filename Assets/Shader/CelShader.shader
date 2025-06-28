Shader "CelShader/CelShaderWithTex"
{
    Properties
    {
        _Darklight("Dark Light",Color) = (0.1,0.1,0.1,1)
        _MainTex("Main Tex",2D) = "white"{}
    	 [Toggle]_ENABLE_ALPHA_TEST("Enable AlphaTest",float)=0
        _Cutoff("Cutoff", Range(0,1)) = 0.5
        _OutlineWidth("OutlineWidth", Range(0, 10)) = 0.4
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        [Toggle]_OLWVWD("OutlineWidth Varies With Distance?", float) = 0
    }
    SubShader
    {
        Pass
        {
            Tags
			{
				"LightMode" = "UniversalForward"
			}
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 
            float3 _Darklight;
            sampler2D _MainTex;
            struct c2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
                float2 uv:TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos:SV_POSITION;
                float3 worldNormal:NORMAL;
                float2 uv:TEXCOORD;
                float3 objViewDir:COLOR1;
                float3 normal:NORMAL2;
            };
 
            v2f vert(c2v input)
            {
                v2f output;
                output.pos = TransformObjectToHClip((input.vertex));
                output.worldNormal = normalize( mul((float3x3)unity_ObjectToWorld,input.normal) );
                output.uv = input.uv;
 
 
                float3 ObjViewDir = TransformWorldToObject(GetCameraPositionWS()) - TransformObjectToWorld(input.vertex);
                output.objViewDir = ObjViewDir;
                output.normal = normalize(input.normal);
 
                return output;
            }
 
 
            half3 frag(in v2f input):SV_TARGET0
            {
                half3 diffuseColor = _MainLightColor.rgb *  tex2D(_MainTex, input.uv).rgb ;
                half3 col = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;
                
                
                
                half3 worldLightDir = normalize(_MainLightPosition.xyz);
 
 
                if(dot(input.worldNormal,worldLightDir)<0)
                {
                    return col * _Darklight;
                }
                return col;
            }
 
            
            ENDHLSL
        }


         Pass 
         {
            Name "OutLine"
            Tags{ "LightMode" = "SRPDefaultUnlit" }
		    Cull front
		    HLSLPROGRAM
		     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"         
		     #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
		     #pragma shader_feature _ENABLE_ALPHA_TEST_ON
		     #pragma shader_feature _OLWVWD_ON
		     #pragma vertex vert  
		     #pragma fragment frag

		     float4 _OutlineColor;
		     float _OutlineWidth;
		     struct Attributes
		     {
	            float4 positionOS : POSITION;
	            float4 normalOS : NORMAL;
	            float4 texcoord : TEXCOORD;
		     };
		     struct Varyings
		     {
	            float4 positionCS : SV_POSITION;
	            float2 uv : TEXCOORD1;
		     };
		    Varyings vert(Attributes input)
		     {
	            float4 scaledScreenParams = GetScaledScreenParams();
	            float ScaleX = abs(scaledScreenParams.x / scaledScreenParams.y);//求得X因屏幕比例缩放的倍数
	            Varyings output;
		        VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
	            VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
	            float3 normalCS = TransformWorldToHClipDir(normalInput.normalWS);//法线转换到裁剪空间
	            float2 extendDis = normalize(normalCS.xy) *(_OutlineWidth*0.01);//根据法线和线宽计算偏移量
	            extendDis.x /=ScaleX ;//由于屏幕比例可能不是1:1，所以偏移量会被拉伸显示，根据屏幕比例把x进行修正
	            output.positionCS = vertexInput.positionCS;
	            #if _OLWVWD_ON
	                //屏幕下描边宽度会变
	                output.positionCS.xy +=extendDis;
	            #else
	                //屏幕下描边宽度不变，则需要顶点偏移的距离在NDC坐标下为固定值
	                //因为后续会转换成NDC坐标，会除w进行缩放，所以先乘一个w，那么该偏移的距离就不会在NDC下有变换
	                output.positionCS.xy += extendDis * output.positionCS.w ;
	            #endif
		        return output;
		     }
		     float4 frag(Varyings input) : SV_Target
		     {
	            return float4(_OutlineColor.rgb, 1);
		     }
		     ENDHLSL
		}
    }
    FallBack "Diffuse"
}
 
 
//LZX completed this shader in 2024/04/04